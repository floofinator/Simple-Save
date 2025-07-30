using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.IO.Compression;
namespace Floofinator.SimpleSave
{
    public static class SceneFiler
    {
        public static Filer Filer;
        static float _progress = 0;
        public static float Progress {
            get => _progress;
            private set
            {
                _progress = value;
                OnProgressChanged?.Invoke(value);
            }
        }
        public static event Action<float> OnProgressChanged;
        static float LastProgressIncrement = 1.0f;
        static float ProgressIncrement = 1.0f;
        public enum ProgressStage
        {
            IDLE, INSTANCING, LOADING, SAVING
        }
        static ProgressStage _stage;
        public static ProgressStage Stage
        {
            get => _stage;
            private set
            {
                _stage = value;
                OnStageChanged?.Invoke(value);
            }
        }
        public static event Action<ProgressStage> OnStageChanged;
        public static event Action OnLoadFinish,OnLoadStart,OnSaveFinish,OnSaveStart;
        public static bool LogVerbose = false;
        public static void InitializeIdentification()
        {
            IdentifiedBehaviour.ID_DICTIONARY.Clear();
            IdentifiedBehaviour[] all = GameObject.FindObjectsOfType<IdentifiedBehaviour>(true);
            foreach (IdentifiedBehaviour identity in all)
            {
                identity.IdentifyParent();
            }
            foreach (IdentifiedBehaviour identity in all)
            {
                identity.AddToDictionary();
            }
        }
        static void SetSceneActive(bool active)
        {
            
            foreach (IdentifiedBehaviour identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
            {
                identity.gameObject.SetActive(active);
            }
        }
        public static void SaveInstant(string sceneName)
        {
            IEnumerator saveRoutine = Save(sceneName);
            while (saveRoutine.MoveNext())
            {
                // execute entire coroutine instantly
            }
        }
        public static IEnumerator Save(string sceneName)
        {
            Progress = 0;
            Stage = ProgressStage.SAVING;
            OnSaveStart?.Invoke();

            Filer.CreateDirectory(sceneName);

            foreach (IdentifiedBehaviour identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
            {
                if (identity is not ISaveable saveable) continue;

                string dataPath = "";
                IdentifiedObject parent = identity.ParentObject;
                while (parent)
                {
                    dataPath = Path.Combine(parent.ID, dataPath);
                    if (parent is IdentifiedInstance instance) dataPath = Path.Combine('#' + instance.ResourcePath, dataPath);
                    parent = parent.ParentObject;
                }

                string directory = Path.Combine(sceneName, dataPath);
                if (!Filer.DirectoryExists(directory)) Filer.CreateDirectory(directory);

                Filer.SaveFile(directory, $"{identity.ID}.save", saveable.Save());

                yield return null;
            }

            if (LogVerbose) Debug.Log("Data for scene \"" + sceneName + "\" saved.");

            Stage = ProgressStage.IDLE;

            OnSaveFinish?.Invoke();
        }
        public static void ClearScene(string sceneName)
        {
            Filer.DeleteDirectory(sceneName);

            if (LogVerbose) Debug.Log($"Cleared save data for \"{sceneName}\"");
        }

        public static bool HasScene(string sceneName)
        {
            return Filer.DirectoryExists(sceneName);
        }

        public static void DivideProgressFraction(string directory)
        {
            var files = Filer.GetDirectories(directory);
            LastProgressIncrement = ProgressIncrement;
            if (files.Length > 0)
            {
                ProgressIncrement /= files.Length;  
            }
        }
        public static void RevertProgressFraction()
        {
            ProgressIncrement = LastProgressIncrement;
        }
        public static void LoadInstant(string sceneName)
        {
            IEnumerator loadRoutine = Load(sceneName);
            while (loadRoutine.MoveNext())
            {
                // execute entire coroutine instantly
            }
        }
        public static IEnumerator Load(string sceneName)
        {
            Progress = 0;
            OnLoadStart?.Invoke();

            SetSceneActive(false);
            yield return null;

            //load instances first before loading data so that they can be identified
            yield return LoadDirectoryInstances(sceneName, sceneName);

            yield return null;
            

            yield return LoadDirectory(sceneName, sceneName);

            yield return null;
            SetSceneActive(true);

            if (LogVerbose) Debug.Log("Data for scene \"" + sceneName + "\" loaded.");

            Stage = ProgressStage.IDLE;
            OnLoadFinish?.Invoke();
        }
        static IEnumerator LoadDirectoryInstances(string directory, string sceneName)
        {
            DivideProgressFraction(directory);

            Stage = ProgressStage.INSTANCING;

            foreach (string dataName in Filer.GetDirectories(directory))
            {
                //if this directory is an instanced object, denoted by the '#' as a starting character
                //all the subdirectories are instance id's and need to be re-instanced
                string dataDirectory = Path.Combine(directory, dataName);
                string[] splitName = dataName.Split('#');
                if (splitName.Length > 1)
                {
                    DivideProgressFraction(dataDirectory);
                    foreach (string instanceDataName in Filer.GetDirectories(dataDirectory))
                    {
                        CreateIdentifiedInstance(splitName[1], instanceDataName, directory, sceneName);
                        Progress += ProgressIncrement;
                        yield return null;
                    }
                    RevertProgressFraction();
                }
                yield return LoadDirectoryInstances(dataDirectory, sceneName);
            }

            RevertProgressFraction();
        }
        static IEnumerator LoadDirectory(string directory, string sceneName)
        {
            Stage = ProgressStage.LOADING;

            //load data from files
            yield return LoadFiles(directory, sceneName);

            //look for other directories
            foreach (string dataName in Filer.GetDirectories(directory))
            {
                string dataDirectory = Path.Combine(directory, dataName);
                yield return LoadDirectory(dataDirectory, sceneName);
            }
        }
        static void CreateIdentifiedInstance(string prefabName, string instanceID, string directory, string sceneName)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            if (prefab == null) Debug.LogErrorFormat($"No prefab with name \"{prefabName}\" found in Resources.");

            Transform parent = null;

            string dictionaryID = GetIDFromDirectory(directory, sceneName);
            if (IdentifiedBehaviour.ID_DICTIONARY.TryGetValue(dictionaryID, out IdentifiedBehaviour identity))
            {
                parent = identity.transform;
            }

            GameObject instance = GameObject.Instantiate(prefab);

            instance.transform.SetParent(parent);

            instance.GetComponent<IdentifiedInstance>().AssignInstanceID(instanceID);

            instance.SetActive(false);

            if (LogVerbose) Debug.Log("Instantiated \"" + prefabName + "\" with ID " + instanceID);
        }
        //this needs to be fixed to account for parent ids in the directory heirarchy
        static string GetIDFromDirectory(string directory, string sceneName)
        {
            string parentID = "";
            string[] dataParts = directory.Split('\\');
            foreach (string id in dataParts)
            {
                if (id == sceneName) continue;
                if (id.Contains('#')) continue;
                if (!string.IsNullOrWhiteSpace(parentID)) parentID += '.';
                parentID += id;
            }
            return parentID;
        }
        static IEnumerator LoadFiles(string directory, string sceneName)
        {
            string parentID = GetIDFromDirectory(directory, sceneName);

            DivideProgressFraction(directory);
            foreach (string dataName in Filer.GetFiles(directory))
            {
                string dictionaryID = Path.GetFileNameWithoutExtension(dataName);
                if (!string.IsNullOrWhiteSpace(parentID)) dictionaryID = parentID + "." + dictionaryID;
                if (IdentifiedBehaviour.ID_DICTIONARY.TryGetValue(dictionaryID, out IdentifiedBehaviour identity))
                {
                    if (identity is ISaveable saveable)
                    {
                        if (Filer.LoadFile(directory, $"{identity.ID}.save", saveable.GetSaveType(), out object data))
                        {
                            saveable.Load(data);
                            Progress += ProgressIncrement;
                            yield return null;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"ID \"{dictionaryID}\" not found in dictionary");
                }
            }
            RevertProgressFraction();
        }
    }
}