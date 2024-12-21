using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Floofinator.SimpleSave.Editor
{
    public class SaveEditorWindow : EditorWindow
    {
        JsonFiler filer;

        [MenuItem("Window/Save Editor")]
        static void Init()
        {
            GetWindow<SaveEditorWindow>("Save Editor");
        }
        private void OnEnable()
        {
            filer = new();
        }
        void CreateGUI()
        {
            Button clearButton = new();
            clearButton.Add(new Label("Clear Save"));
            clearButton.clicked += () => SceneFiler.ClearScene(filer);

            rootVisualElement.Add(clearButton);
        }
    }
}
