using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Floofinator.SimpleSave
{
    public abstract class IdentifiedBehaviour : MonoBehaviour
    {
        [SerializeField, HideInInspector] string id = string.Empty;
        public string DictionaryID => GetParentID() + id;
        public string ID => id;
        IdentifiedPrefab parentPrefab;
        public IdentifiedPrefab ParentPrefab => parentPrefab;
        public static Dictionary<string, IdentifiedBehaviour> ID_DICTIONARY = new();
        public void GenerateID() => id = Guid.NewGuid().ToString();
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
        }
        public void RemoveID()
        {
            if (!ID_DICTIONARY.ContainsKey(DictionaryID)) return;
            
            ID_DICTIONARY.Remove(DictionaryID);
        }
        protected virtual void OnDestroy()
        {
            RemoveID();
        }
    }
}
