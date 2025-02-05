using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Floofinator.SimpleSave.Editor
{
    public class SaveEditorWindow : EditorWindow
    {
        [MenuItem("Window/Save Editor")]
        static void Init()
        {
            GetWindow<SaveEditorWindow>("Save Editor");
        }
        void CreateGUI()
        {
            Label label = new ("WIP");
            // Button clearButton = new();
            // clearButton.Add(new Label("Clear Save"));
            // clearButton.clicked += () => SceneFiler.ClearScene(filer);

            // rootVisualElement.Add(clearButton);

            rootVisualElement.Add(label);
        }
    }
}
