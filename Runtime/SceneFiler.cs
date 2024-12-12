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
        public static void SaveScene(Filer filer)
        {
            string path = SceneManager.GetActiveScene().name;

            Debug.Log($"Saving data for scene {path}.");

            foreach (var identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
            {
                if (identity is ISaveable saveable)
                {
                    filer.SaveFile(path, $"{identity.ID}.json", saveable.Save());
                }
            }
        }
        public static void ClearScene(Filer filer)
        {
            string path = SceneManager.GetActiveScene().name;
            filer.DeleteFile(path, "");
        }
        public static bool LoadScene(Filer filer)
        {
            string path = SceneManager.GetActiveScene().name;

            Debug.Log($"Loading data for scene {path}.");

            if (filer.DirectoryExists(path))
            {
                foreach (var identity in IdentifiedBehaviour.ID_DICTIONARY.Values)
                {
                    if (identity is ISaveable saveable)
                    {
                        if (filer.LoadFile(path, $"{identity.ID}.json", saveable.GetSaveType(), out object data))
                        {
                            saveable.Load(data);
                        }
                    }
                }

                return true;
            }
            return false;
        }
    }
}