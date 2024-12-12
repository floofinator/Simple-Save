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
        private void OnDestroy()
        {
            SceneFiler.SaveScene(jsonFiler);
        }
        IEnumerator LoadRoutine()
        {
            //Wait a frame before we load so that any objects that do something on Start can do it first.
            yield return new WaitForEndOfFrame();
            SceneFiler.LoadScene(jsonFiler);
        }
    }
}