using System.Collections.Generic;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    /// <summary>
    /// A base class for MonoBehaviours that can save and load data.
    /// </summary>
    public abstract class IdentifiedBehaviour : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected string id = string.Empty;
        public string DictionaryID => GetParentID() + id;
        public string ID => id;
        public bool HasEmptyID => string.IsNullOrWhiteSpace(ID);
        public IdentifiedObject ParentObject {get; protected set;}
        public static Dictionary<string, IdentifiedBehaviour> ID_DICTIONARY = new();
        public static bool LogVerbose = false;
        public virtual void IdentifyParent()
        {
            ParentObject = GetComponentInParent<IdentifiedObject>(true);
        }
        bool HasValidParents()
        {
            IdentifiedObject parent = ParentObject;
            while (parent)
            {
                if (parent.HasEmptyID) return false;
                parent = parent.ParentObject;
            }
            return true;
        }
        string GetParentID()
        {
            if (ParentObject) return ParentObject.DictionaryID + '.';

            return string.Empty;
        }
        public void AddToDictionary()
        {
            if (HasEmptyID)
            {
                Debug.LogWarning("ID is empty for \"" + gameObject.name + "\" it will not be added.");
                return;
            }
            if (!HasValidParents())
            {
                Debug.LogWarning("ID is empty for a parent of\"" + gameObject.name + "\" it will not be added yet.");
                return;
            }
            if (ID_DICTIONARY.ContainsKey(DictionaryID))
            {
                Debug.LogWarning("Dictionary ID \"" + DictionaryID + "\" already exists for \"" + gameObject.name + "\" so it will not be added.");
                return;
            }
            ID_DICTIONARY.Add(DictionaryID, this);
            if (LogVerbose) print("ID \"" + DictionaryID + "\" added for GameObject \"" + gameObject.name + "\"");
        }
        public void RemoveFromDictionary()
        {
            if (!ID_DICTIONARY.ContainsKey(DictionaryID))
            {
                Debug.LogWarning("Dictionary ID \"" + DictionaryID + "\" does not exist for \"" + gameObject.name + "\" so it cannot be removed.");
                return;
            }
            ID_DICTIONARY.Remove(DictionaryID);
            if (LogVerbose) print("ID \"" + DictionaryID + "\" removed for GameObject \"" + gameObject.name + "\"");
        }
        protected virtual void OnDestroy()
        {
            RemoveFromDictionary();
        }
    }
}
