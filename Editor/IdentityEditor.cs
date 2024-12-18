using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UI;
using UnityEditor.UIElements;

namespace Floofinator.SimpleSave.Editor
{
    [CustomEditor(typeof(IdentifiedBehaviour), true)]
    public class IdentityEditor : UnityEditor.Editor
    {
        IdentifiedBehaviour identity;
        private void OnEnable()
        {
            identity = (IdentifiedBehaviour)target;
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            Button generateButton = new();
            generateButton.Add(new Label("Generate ID"));
            generateButton.style.flexGrow = 1;

            Button clearButton = new();
            clearButton.Add(new Label("Clear ID"));
            clearButton.style.flexGrow = 1;

            generateButton.clicked += () => { Undo.RecordObject(target, "Generated ID"); identity.GenerateID(); };
            clearButton.clicked += () => { Undo.RecordObject(target, "Cleared ID"); identity.ClearID(); };

            VisualElement container = new();
            container.style.flexDirection = FlexDirection.Row;

            container.Add(generateButton);
            container.Add(clearButton);
            root.Add(container);
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            return root;
        }
    }
}
