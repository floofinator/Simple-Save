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
        [SerializeField] bool saveOnDestroy = true;
        public UnityEvent OnNoSave, OnLoaded, OnSaved;
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
        private void OnDestroy()
        {
            if (saveOnDestroy) Save();
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