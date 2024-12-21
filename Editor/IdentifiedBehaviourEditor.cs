using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Floofinator.SimpleSave.Editor
{
    [CustomEditor(typeof(IdentifiedBehaviour), true)]
    public class IdentifiedBehaviourEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            TextField idField = new("Save ID")
            {
                name = "id",
                bindingPath = "id"
            };

            idField.style.flexGrow = 1;

            root.Add(idField);
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            root.Bind(serializedObject);

            return root;
        }
    }
}
