using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Floofinator.SimpleSave
{
    public class SceneLoadSave : MonoBehaviour
    {
        JsonFiler jsonFiler;
        void Start()
        {
            jsonFiler = new();
            StartCoroutine(LoadRoutine());
        }
        private void OnApplicationQuit()
        {
            SceneFiler.SaveScene(jsonFiler);
        }
        IEnumerator LoadRoutine()
        {
            //Wait to the end of the frame before we load so that any objects that do something on Start can do it first.
            yield return new WaitForEndOfFrame();
            
            bool hasSave = SceneFiler.LoadScene(jsonFiler);
            Debug.Log("Save File: " + hasSave);
        }
    }
}