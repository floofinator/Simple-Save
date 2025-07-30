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
        [SerializeField] bool logVerbose;
        [SerializeField] bool loadOnStart = true;
        [SerializeField] bool saveOnQuit = true;
        public UnityEvent OnNoData, OnLoaded, OnSaved;
        string sceneName = "";

        void Start()
        {
            SceneFiler.LogVerbose = logVerbose;
            IdentifiedBehaviour.LogVerbose = logVerbose;
            SceneFiler.InitializeIdentification();
            sceneName = SceneManager.GetActiveScene().name;
            if (loadOnStart)
            {
                StartCoroutine(Load());
            }
        }

        public IEnumerator Load()
        {
            if (SceneFiler.HasScene(sceneName))
            {
                SceneFiler.Filer.UnCompress();
                yield return SceneFiler.Load(sceneName);
                SceneFiler.Filer.DeleteUnCompress();
                OnLoaded?.Invoke();
                if (logVerbose)
                {
                    Debug.Log("loaded scene data.");
                }
            }
            else
            {
                OnNoData?.Invoke();
                if (logVerbose)
                {
                    Debug.Log("No scene data.");
                }
            }
        }

        void OnApplicationQuit()
        {
            if (saveOnQuit)
            {
                SceneFiler.SaveInstant(sceneName);
            }
        }

        public IEnumerator Save()
        {
            yield return SceneFiler.Save(sceneName);
            SceneFiler.Filer.Compress();
            OnSaved?.Invoke();
            if (logVerbose)
            {
                Debug.Log("Saved scene data.");
            }
        }
    }
}