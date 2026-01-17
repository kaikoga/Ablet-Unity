using System.Collections.Generic;
using Ablet.Models.Serialized;
using Ablet.Registries;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    class LayerReportView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/LayerReportView.uxml";

        readonly Label _layerDisplayNameText;
        readonly VisualElement _resultContainer;

        public LayerReportView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            _layerDisplayNameText = container.Q<Label>("layerDisplayNameText");
            _resultContainer = container.Q<VisualElement>("resultContainer");
            hierarchy.Add(container);
        }

        public void Draw(string layerId, IEnumerable<SerializedBuildReport> buildReports, GameObject? entrypointObject)
        {
            _layerDisplayNameText.text = LayerRegistry.Instance.ToDisplayName(layerId);
            _resultContainer.Clear();
            style.display = DisplayStyle.None;
            foreach (var buildReport in buildReports)
            {
                var reporter = BuildReporterRegistry.Instance.For(buildReport.payload);
                if (reporter.IsEnabled && reporter.Render(buildReport.payload, entrypointObject) is { } payloadView)
                {
                    _resultContainer.Add(payloadView);
                    style.display = DisplayStyle.Flex;
                }
            }
        }
    }
}
