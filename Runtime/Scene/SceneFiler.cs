using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using System;
namespace Floofinator.SimpleSave
{
    public static class SceneFiler
    {
        const string SCENE_SAVE = "Scene";
        static string GetSceneName() => SceneManager.GetActiveScene().name;
        public static void InitialIdentification()
        {
            foreach (var identity in GameObject.FindObjectsOfType<IdentifiedBehaviour>())
            {
                identity.AddID();
            }
        }
        public static void SaveScene(Filer filer)
        {
            string sceneName = GetSceneName();

            filer.DeleteDirectory(sceneName);

            foreach (var identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
            {
                if (identity is not ISaveable saveable) continue;

                string sceneDataName = SCENE_SAVE;
                if (identity.ParentPrefab) sceneDataName = identity.ParentPrefab.ResourcePath + '.' + identity.ParentPrefab.ID;

                string directory = Path.Combine(sceneName, sceneDataName);
                if (!filer.DirectoryExists(directory)) filer.CreateDirectory(directory);

                filer.SaveFile(directory, $"{identity.ID}.json", saveable.Save());
            }
        }
        public static void ClearScene(Filer filer)
        {
            string sceneName = GetSceneName();

            filer.DeleteDirectory(sceneName);

            Debug.Log($"Cleared save data for \"{sceneName}\"");
        }
        public static bool LoadScene(Filer filer)
        {
            string sceneName = GetSceneName();

            if (!filer.DirectoryExists(sceneName)) return false;

            foreach (var sceneDataName in filer.GetDirectories(sceneName))
            {
                if (sceneDataName.Equals(SCENE_SAVE)) LoadStaticData(filer, sceneName);
                else LoadDynamicData(filer, sceneName, sceneDataName);
            }

            return true;
        }
        static void LoadStaticData(Filer filer, string sceneName)
        {
            string dataDirectory = Path.Combine(sceneName, SCENE_SAVE);
            LoadFiles(filer, dataDirectory);
        }
        static void LoadDynamicData(Filer filer, string sceneName, string dataName)
        {
            string[] dataParts = Path.GetFileName(dataName).Split('.');

            GameObject prefab = Resources.Load<GameObject>(dataParts[0]);

            if (prefab == null) Debug.LogErrorFormat($"No prefab with name \"{dataParts[0]}\" found in Resources.");
            GameObject instance = GameObject.Instantiate(prefab);

            instance.GetComponent<IdentifiedPrefab>().AssignInstanceID(dataParts[1]);

            string dataDirectory = Path.Combine(sceneName, dataName);
            LoadFiles(filer, dataDirectory);
        }
        static void LoadFiles(Filer filer, string directory)
        {
            string parentID = "";
            string[] dataParts = directory.Split('.');
            if (dataParts.Length > 1) parentID = dataParts[1] + '.';

            foreach (var fileIDName in filer.GetFiles(directory))
            {
                string dictionaryID = parentID + Path.GetFileNameWithoutExtension(fileIDName);
                if (IdentifiedBehaviour.ID_DICTIONARY.TryGetValue(dictionaryID, out IdentifiedBehaviour identity))
                {
                    if (identity is ISaveable saveable)
                    {
                        if (filer.LoadFile(directory, $"{identity.ID}.json", saveable.GetSaveType(), out object data))
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