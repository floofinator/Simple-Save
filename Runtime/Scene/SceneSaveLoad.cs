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
        public UnityEvent OnNoSave, OnLoaded, OnSaved;
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
                    yield return SceneFiler.LoadScene(sceneName);
                    OnLoaded?.Invoke();
                }
                else
                    OnNoSave?.Invoke();
            }
        }
        private void OnApplicationQuit()
        {
            if (saveOnQuit) Save();
        }
        public void Save()
        {
            if (SceneFiler.Filer == null)
            {
                Debug.LogError("No filer instance to save with! Please set the filer before attempting to save the scene.");
                return;
            }

            IEnumerator saveRoutine = SceneFiler.SaveScene(sceneName);
            while (saveRoutine.MoveNext())
            {
                // execute entire coroutine instantly
            }
            OnSaved?.Invoke();
        }
    }
}