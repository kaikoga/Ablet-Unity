using System.Collections.Generic;
using Ablet.Debugging.View.UIElements;
using Ablet.Planning;
using UnityEditor;
using UnityEngine;

namespace Ablet.Debugging.View.Windows
{
    class DebugBuildPlanWindow : EditorWindow
    {
        DebugBuildPlanWindowView? _view;

        internal static void ShowWindow(IEnumerable<AbletPass> plan)
        {
            var window = GetWindow<DebugBuildPlanWindow>() ?? CreateInstance<DebugBuildPlanWindow>();
            window.Show();
            window._view?.Draw(plan);
        }

        void CreateGUI()
        {
            titleContent = new GUIContent("Ablet Debug Build Plan");
            minSize = new Vector2(300f, 400f);
            _view = new DebugBuildPlanWindowView();
            rootVisualElement.Add(_view);
        }
    }
}
