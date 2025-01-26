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
        public UnityEvent OnLoaded, OnSaved;
        JsonFiler jsonFiler;
        void Start()
        {
            jsonFiler = new();
            SceneFiler.InitialIdentification();
            SceneFiler.LoadScene(jsonFiler);

            OnLoaded?.Invoke();
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