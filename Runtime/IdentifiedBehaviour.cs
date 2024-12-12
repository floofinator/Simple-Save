using System.Collections.Generic;
using UnityEngine;
using System;

namespace Floofinator.SimpleSave
{
    public abstract class IdentifiedBehaviour : MonoBehaviour
    {
        [ContextMenu("Generate ID")] public void GenerateID() => id = Guid.NewGuid().ToString();
        [SerializeField] string id = string.Empty;
        public string ID => id;
        public static Dictionary<string, IdentifiedBehaviour> ID_DICTIONARY = new();
        protected virtual void Awake()
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                Debug.LogWarning("ID is empty for " + gameObject.name + ". This will not be added to the dictionary.");
                return;
            }
            ID_DICTIONARY.Add(ID, this);
        }
        protected virtual void OnDestroy()
        {
            ID_DICTIONARY.Remove(ID);
        }
    }
}
