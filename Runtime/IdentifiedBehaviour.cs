using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public abstract class IdentifiedBehaviour : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected string id = string.Empty;
        public string DictionaryID => GetParentID() + id;
        public string ID => id;
        public bool HasEmptyID => string.IsNullOrWhiteSpace(ID);
        IdentifiedPrefab parentPrefab = null;
        public IdentifiedPrefab ParentPrefab => parentPrefab;
        public static Dictionary<string, IdentifiedBehaviour> ID_DICTIONARY = new();
        string GetParentID()
        {
            if (parentPrefab == null) parentPrefab = TryGetParent();
            if (parentPrefab == this)
            {
                Debug.LogError("OOPS! The parent of this identity was somehow assigned to itself! This shouldn't happen!");
                parentPrefab = null;
            }

            if (parentPrefab)
            {
                return parentPrefab.DictionaryID + '.';
            }

            return string.Empty;
        }
        IdentifiedPrefab TryGetParent()
        {
            if (this is IdentifiedPrefab)
            {
                if (transform.parent == null) return null;
                //if this is a prefab id and we want to see if there is one in parent
                return transform.parent.GetComponentInParent<IdentifiedPrefab>();
            }
            else
            {
                //if this is not a prefab id
                return GetComponentInParent<IdentifiedPrefab>();
            }
        }
        public void AddID()
        {
            if (HasEmptyID)
            {
                Debug.LogWarning("ID is empty for \"" + gameObject.name + "\" it will not be added.");
                return;
            }
            if (ID_DICTIONARY.ContainsKey(DictionaryID))
            {
                Debug.LogWarning("ID \"" + DictionaryID + "\" already exists for \"" + gameObject.name + "\" it will not be added.");
                return;
            }
            ID_DICTIONARY.Add(DictionaryID, this);
        }
        public void RemoveID()
        {
            if (!ID_DICTIONARY.ContainsKey(DictionaryID))
            {
                Debug.LogWarning("ID \"" + DictionaryID + "\" does not exist for \"" + gameObject.name + "\" so it cannot be removed.");
                return;
            }
            ID_DICTIONARY.Remove(DictionaryID);
        }
        protected virtual void OnDestroy()
        {
            RemoveID();
        }
    }
}
