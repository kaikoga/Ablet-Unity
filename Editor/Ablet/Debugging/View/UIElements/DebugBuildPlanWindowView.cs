using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ablet.Planning;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Debugging.View.UIElements
{
    public class DebugBuildPlanWindowView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Debugging/View/Uxml/DebugBuildPlanWindowView.uxml";

        AbletPass[] _passes = Array.Empty<AbletPass>();
        readonly ListView _passList;

        public DebugBuildPlanWindowView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            var copyButton = container.Q<Button>("copyButton");
            copyButton.clicked += OnCopy;
            _passList = container.Q<ListView>("passList");
            _passList.itemsSource = Array.Empty<AbletPass>();
            _passList.makeItem += () => new DebugAbletPassView();
            _passList.bindItem += (view, index) => ((DebugAbletPassView)view).Draw(_passes[index]);
            hierarchy.Add(container);
        }

        public void Draw(IEnumerable<AbletPass> passes)
        {
            _passes = passes.ToArray();
            _passList.itemsSource = _passes;
        }

        void OnCopy()
        {
            GUIUtility.systemCopyBuffer = _passes
                .Select(DebugAbletPassView.Format)
                .Aggregate(new StringBuilder(), (sb, pass) => sb.AppendLine(pass))
                .ToString();
        }
    }
}
