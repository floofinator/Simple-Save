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
        void Start()
        {
            jsonFiler = new();
            SceneFiler.InitialIdentification();
            bool loaded = SceneFiler.LoadScene(jsonFiler);

            if (loaded) OnLoaded?.Invoke();
            else OnNoSave?.Invoke();
        }
        private void OnApplicationQuit()
        {
            SceneFiler.SaveScene(jsonFiler);
            OnSaved?.Invoke();
        }
        public void Save()
        {
            SceneFiler.SaveScene(jsonFiler);
            OnSaved?.Invoke();
        }
    }
}