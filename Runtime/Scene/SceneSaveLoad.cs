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
        [SerializeField] bool compressSave = true;
        public UnityEvent OnNoData, OnLoaded, OnSaved;
        string sceneName = "";

        void Start()
        {
            SceneFiler.StayActive.Add(gameObject);

            SceneFiler.LogVerbose = logVerbose;
            IdentifiedBehaviour.LogVerbose = logVerbose;

            SceneFiler.InitializeIdentification();
            sceneName = SceneManager.GetActiveScene().name;
            if (loadOnStart)
            {
                StartCoroutine(Load());
            }
        }

        void OnDestroy()
        {
            SceneFiler.StayActive.Remove(gameObject);
        }

        public IEnumerator Load()
        {
            if (compressSave) SceneFiler.Filer.UnCompress();
            if (SceneFiler.HasScene(sceneName))
            {
                yield return SceneFiler.Load(sceneName);

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
            if (compressSave) SceneFiler.Filer.DeleteUnCompress();
        }

        void OnApplicationQuit()
        {
            if (saveOnQuit)
            {
                SceneFiler.SaveInstant(sceneName);
                if (compressSave) SceneFiler.Filer.Compress();
            }
        }

        public IEnumerator Save()
        {
            yield return SceneFiler.Save(sceneName);
            if (compressSave) SceneFiler.Filer.Compress();
            OnSaved?.Invoke();
            if (logVerbose)
            {
                Debug.Log("Saved scene data.");
            }
        }
    }
}