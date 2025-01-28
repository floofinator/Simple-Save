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
        public JsonFiler Filer => jsonFiler;
        public UnityEvent OnNoSave, OnLoaded, OnSaved;
        JsonFiler jsonFiler;
        private void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        void Start()
        {
            jsonFiler = new();
            bool loaded = SceneFiler.LoadScene(jsonFiler);

            if (loaded) OnLoaded?.Invoke();
            else OnNoSave?.Invoke();
        }
        private void OnDestroy()
        {
            print("Destroyed");
        }
        private void OnSceneUnloaded(Scene arg0)
        {
            print("Unloaded");
            Save();
        }
        private void OnApplicationQuit()
        {
            print("Quit");
            Save();
        }
        public void Save()
        {
            SceneFiler.SaveScene(jsonFiler);
            OnSaved?.Invoke();
        }
    }
}