using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Floofinator.SimpleSave
{
    public class SceneSaveLoad : MonoBehaviour
    {
        [SerializeField] bool loadOnStart = true;
        [SerializeField] bool saveOnQuit = true;
        [SerializeField] bool saveOnSceneUnload = true;
        public UnityEvent OnNoSave, OnLoaded, OnSaved;
        private void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        void Start()
        {
            if (loadOnStart) Load();
        }
        public bool Load()
        {
            if (Filer.Instance == null)
            {
                Debug.LogError("No filer instance to load with! Please set the filer before attempting to load the scene.");
                return false;
            }

            bool loaded = SceneFiler.LoadScene(Filer.Instance);

            if (loaded) OnLoaded?.Invoke();
            else OnNoSave?.Invoke();

            return loaded;
        }
        private void OnSceneUnloaded(Scene arg0)
        {
            if (saveOnSceneUnload) Save();
        }
        private void OnApplicationQuit()
        {
            if (saveOnQuit) Save();
        }
        public void Save()
        {
            if (Filer.Instance == null)
            {
                Debug.LogError("No filer instance to save with! Please set the filer before attempting to save the scene.");
                return;
            }

            SceneFiler.SaveScene(Filer.Instance);
            OnSaved?.Invoke();
        }
    }
}