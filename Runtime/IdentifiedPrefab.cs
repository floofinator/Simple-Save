using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Floofinator.SimpleSave
{
    public class IdentifiedPrefab : IdentifiedBehaviour
    {
        [SerializeField] string resourcePath;
        public string ResourcePath => resourcePath;
        public bool Instanced {get; private set;}
        private void Start()
        {
            //if this prefab is instantiated without an id, we need to create one.
            if (HasEmptyID) GenerateInstanceID();
        }
        public void AssignInstanceID(string newId)
        {
            id = newId;
            Debug.Log("Prefab assigned id: \"" + id + "\"");
            foreach (var identified in GetComponentsInChildren<IdentifiedBehaviour>())
            {
                identified.AddID();
            }
            Instanced = true;
        }
        public void GenerateInstanceID()
        {
            AssignInstanceID(Guid.NewGuid().ToString());
        }
    }
}
