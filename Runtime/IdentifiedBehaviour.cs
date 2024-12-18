using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Floofinator.SimpleSave
{
    public abstract class IdentifiedBehaviour : MonoBehaviour
    {
        [ContextMenu("Generate ID")] public void GenerateID() => id = Guid.NewGuid().ToString();
        [ContextMenu("Clear ID")] public void ClearID() => id = string.Empty;
        [SerializeField] string id = string.Empty;
        public string DictionaryID => GetParentID() + id;
        public string ID => id;
        IdentifiedPrefab parentPrefab;
        public IdentifiedPrefab ParentPrefab => parentPrefab;
        public static Dictionary<string, IdentifiedBehaviour> ID_DICTIONARY = new();
        string GetParentID()
        {
            if (parentPrefab == null) return string.Empty;
            return parentPrefab.DictionaryID + '.';
        }
        protected virtual void Awake()
        {
            parentPrefab = GetComponentInParent<IdentifiedPrefab>();
            if (parentPrefab == this) parentPrefab = null;
        }
        protected virtual void Start()
        {
            //if we have an ID do not generate a new one.
            if (string.IsNullOrWhiteSpace(id))
            {
                GenerateID();
                Debug.LogWarning("ID for " + gameObject.name + " if blank. This object will be treated as dynamic, and be given a new ID.");
            }

            AddID();
        }
        public void SetID(string newId)
        {
            RemoveID();
            id = newId;
            AddID();
        }
        public void AddID()
        {
            if (ID_DICTIONARY.ContainsKey(DictionaryID)) return;
            
            ID_DICTIONARY.Add(DictionaryID, this);

            print(DictionaryID + " added to dictionary.");
        }
        public void RemoveID()
        {
            if (!ID_DICTIONARY.ContainsKey(DictionaryID)) return;
            
            ID_DICTIONARY.Remove(DictionaryID);

            print(DictionaryID + " removed from dictionary.");
        }
        protected virtual void OnDestroy()
        {
            RemoveID();
        }
    }
}
