using System.Collections.Generic;
using System.Text;
using Ablet.Models.Serialized;
using Ablet.Registries;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Ablet.Loch.Tools.AbletLochTool;

namespace Ablet.View.UIElements
{
    class LayerReportView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/LayerReportView.uxml";

        readonly Label _layerDisplayNameText;
        readonly Button _copyButton;
        readonly VisualElement _resultContainer;

        readonly List<SerializedBuildReport> _copyableBuildReports = new List<SerializedBuildReport>();

        public LayerReportView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            container.Localize<LayerReportView>();

            _layerDisplayNameText = container.Q<Label>("layerDisplayNameText");
            _copyButton = container.Q<Button>("copyButton");
            _copyButton.clickable.clicked += OnCopy;
            _resultContainer = container.Q<VisualElement>("resultContainer");
            hierarchy.Add(container);
        }

        public void Draw(string layerId, IEnumerable<SerializedBuildReport> buildReports, GameObject? entrypointObject)
        {
            _layerDisplayNameText.text = LayerRegistry.Instance.ToDisplayName(layerId);
            _resultContainer.Clear();
            _copyableBuildReports.Clear();
            style.display = DisplayStyle.None;
            foreach (var buildReport in buildReports)
            {
                var reporter = BuildReporterRegistry.Instance.For(buildReport.payload);
                if (reporter.IsEnabled && reporter.Render(buildReport.payload, entrypointObject) is { } payloadView)
                {
                    _resultContainer.Add(payloadView);
                    style.display = DisplayStyle.Flex;
                }
                if (buildReport.IsCopyable)
                {
                    _copyableBuildReports.Add(buildReport);
                }
            }
            _copyButton.style.display = _copyableBuildReports.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void OnCopy()
        {
            var sb = new StringBuilder();
            foreach (var buildReport in _copyableBuildReports)
            {
                sb.AppendLine(buildReport.ToCopyableString());
            }
            EditorGUIUtility.systemCopyBuffer = sb.ToString();
        }
    }
}
