using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Floofinator.SimpleSave
{
    public class SceneSaveLoad : MonoBehaviour
    {
        [SerializeField] bool loadOnStart = true;
        [SerializeField] bool saveOnQuit = true;
        public UnityEvent OnNoData, OnLoaded, OnSaved;
        string sceneName = "";
        void Start()
        {
            sceneName = SceneManager.GetActiveScene().name;
            if (loadOnStart)
            {
                StartCoroutine(Load());
            }
        }
        public IEnumerator Load()
        {
            if (SceneFiler.Filer == null)
            {
                Debug.LogError("No filer instance to load with! Please set the scene filer before attempting to load the scene.");
            }
            else
            {
                bool hasSave = SceneFiler.HasScene(sceneName);

                if (hasSave)
                {
                    yield return SceneFiler.Load(sceneName);
                    OnLoaded?.Invoke();
                }
                else
                    OnNoData?.Invoke();
            }
        }
        private void OnApplicationQuit()
        {
            if (saveOnQuit) SceneFiler.SaveInstant(sceneName);
        }
        public IEnumerator Save()
        {
            if (SceneFiler.Filer == null)
            {
                Debug.LogError("No filer instance to save with! Please set the filer before attempting to save the scene.");
            }
            else
            {
                yield return SceneFiler.Save(sceneName);
                OnSaved?.Invoke();
            }
        }
    }
}