using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Floofinator.SimpleSave.Editor
{
    [CustomEditor(typeof(IdentifiedPrefab))]
    public class IdentifiedPrefabEditor : UnityEditor.Editor
    {
        IdentifiedBehaviour identity;
        private void OnEnable()
        {
            identity = (IdentifiedBehaviour)target;
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            return root;
        }
    }
}
