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
            //Wait to the end of the frame before we load so that the dictionary can be established first.
            yield return new WaitForEndOfFrame();

            SceneFiler.LoadScene(jsonFiler);
        }
    }
}