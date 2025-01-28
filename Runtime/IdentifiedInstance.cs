using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Floofinator.SimpleSave
{
    /// <summary>
    /// An extension of IdentifiedObject that is specifically for instantiated prefabs that do not exist in a scene.
    /// Used by the scene filer to identify objects that need to be reinstanced on scene load.
    /// </summary>
    public class IdentifiedInstance : IdentifiedObject
    {
        [SerializeField] string resourcePath;
        public string ResourcePath => resourcePath;
        [ContextMenu("Set Path To Name")]
        void SetPathToName()
        {
            resourcePath = gameObject.name;
        }
        private void Awake()
        {
            foreach (var identified in GetComponentsInChildren<IdentifiedBehaviour>())
            {
                identified.IdentifyParent();
            }
        }
        private void Start()
        {
            //if this prefab is instantiated without an id, we need to create one.
            if (HasEmptyID) GenerateInstanceID();
        }
        public void AssignInstanceID(string newId)
        {
            id = newId;
            // Debug.Log("Prefab assigned id: \"" + id + "\"");
            foreach (var identified in GetComponentsInChildren<IdentifiedBehaviour>())
            {
                identified.AddToDictionary();
            }
        }
        public void GenerateInstanceID()
        {
            AssignInstanceID(Guid.NewGuid().ToString());
        }
    }
}
