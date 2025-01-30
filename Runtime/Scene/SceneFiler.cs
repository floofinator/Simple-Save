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
        static string GetSceneName() => SceneManager.GetActiveScene().name;
        static void InitializeIdentification()
        {
            IdentifiedBehaviour.ID_DICTIONARY.Clear();
            IdentifiedBehaviour[] all = GameObject.FindObjectsOfType<IdentifiedBehaviour>();
            foreach (var identity in all)
            {
                identity.IdentifyParent();
            }
            foreach (var identity in all)
            {
                identity.AddToDictionary();
            }
        }
        public static void SaveScene(Filer filer)
        {
            string sceneName = GetSceneName();

            filer.DeleteDirectory(sceneName);

            foreach (var identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
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
                if (!filer.DirectoryExists(directory)) filer.CreateDirectory(directory);

                filer.SaveFile(directory, $"{identity.ID}.save", saveable.Save());
            }

            Debug.Log("Data saved.");

            filer.Compress();
        }
        public static void ClearScene(Filer filer)
        {
            string sceneName = GetSceneName();

            filer.DeleteDirectory(sceneName);

            Debug.Log($"Cleared save data for \"{sceneName}\"");
        }
        public static bool LoadScene(Filer filer)
        {
            filer.UnCompress();

            InitializeIdentification();

            string sceneName = GetSceneName();

            if (!filer.DirectoryExists(sceneName)) return false;

            //load instances first before loading data so that they can be identified
            LoadDirectoryInstances(filer, sceneName);
            Debug.Log("Instances re-instantiated.");
            LoadDirectory(filer, sceneName);
            Debug.Log("Data loaded.");

            filer.CleanUpUnCompress();

            return true;
        }
        static void LoadDirectoryInstances(Filer filer, string directory)
        {
            foreach (var dataName in filer.GetDirectories(directory))
            {
                //if this directory is an instanced object, denoted by the '#' as a starting character
                //all the subdirectories are instance id's and need to be re-instanced
                string dataDirectory = Path.Combine(directory,dataName);
                string[] splitName = dataName.Split('#');
                if (splitName.Length > 1)
                {
                    foreach (var instanceDataName in filer.GetDirectories(dataDirectory))
                    {
                        CreateIdentifiedInstance(splitName[1], instanceDataName, directory);
                    }
                }
                LoadDirectoryInstances(filer, dataDirectory);
            }
        }
        static void LoadDirectory(Filer filer, string directory)
        {
            //load data from files
            LoadFiles(filer, directory);
            //look for other directories
            foreach (var dataName in filer.GetDirectories(directory))
            {
                string dataDirectory = Path.Combine(directory,dataName);
                LoadDirectory(filer, dataDirectory);
            }
        }
        static void CreateIdentifiedInstance(string prefabName, string instanceID, string directory)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            if (prefab == null) Debug.LogErrorFormat($"No prefab with name \"{prefabName}\" found in Resources.");

            Transform parent = null;

            string dictionaryID = GetIDFromDirectory(directory);
            if (IdentifiedBehaviour.ID_DICTIONARY.TryGetValue(dictionaryID, out IdentifiedBehaviour identity))
            {
                parent = identity.transform;
            }

            GameObject instance = GameObject.Instantiate(prefab, parent);

            instance.GetComponent<IdentifiedInstance>().AssignInstanceID(instanceID);
        }
        //this needs to be fixed to account for parent ids in the directory heirarchy
        static string GetIDFromDirectory(string directory)
        {
            string parentID = "";
            string[] dataParts = directory.Split('\\');
            foreach (string id in dataParts)
            {
                if (id == GetSceneName()) continue;
                if (id.Contains('#')) continue;
                if (!string.IsNullOrWhiteSpace(parentID)) parentID += '.';
                parentID += id;
            }
            return parentID;
        }
        static void LoadFiles(Filer filer, string directory)
        {
            string parentID = GetIDFromDirectory(directory);

            foreach (var dataName in filer.GetFiles(directory))
            {
                string dictionaryID = parentID + "." + Path.GetFileNameWithoutExtension(dataName);
                if (IdentifiedBehaviour.ID_DICTIONARY.TryGetValue(dictionaryID, out IdentifiedBehaviour identity))
                {
                    if (identity is ISaveable saveable)
                    {
                        if (filer.LoadFile(directory, $"{identity.ID}.save", saveable.GetSaveType(), out object data))
                        {
                            saveable.Load(data);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"ID \"{dictionaryID}\" not found in dictionary");
                }
            }
        }
    }
}