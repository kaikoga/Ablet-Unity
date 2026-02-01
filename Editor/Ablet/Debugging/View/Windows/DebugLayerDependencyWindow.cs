using Ablet.Debugging.View.UIElements;
using UnityEditor;
using UnityEngine;

namespace Ablet.Debugging.View.Windows
{
    class DebugLayerDependencyWindow : EditorWindow
    {
        [MenuItem("Tools/Ablet/Debug/Debug Layer Dependency", false, 100)]
        static void ShowWindow()
        {
            (GetWindow<DebugLayerDependencyWindow>() ?? CreateInstance<DebugLayerDependencyWindow>()).Show();
        }

        void CreateGUI()
        {
            titleContent = new GUIContent("Ablet Debug Layer Dependency");
            minSize = new Vector2(600f, 400f);
            rootVisualElement.Add(new DebugLayerDependencyWindowView());
        }
    }
}
