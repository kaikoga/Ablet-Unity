using System.Collections.Generic;
using Ablet.Loch.Tools;
using Ablet.Models.Serialized;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    class AbletConsoleWindowView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/AbletConsoleWindowView.uxml";
        const string UssPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/Ablet.uss";

        public readonly BuildReporterListView BuildReporterList;
        public readonly ListView EntrypointList;
        public readonly ToolbarButton ClearReportsButton;
        public readonly GameObjectPopupField SceneEntrypointsPopup;
        public readonly ObjectField EntrypointObjectField;
        readonly VisualElement _buildReportsContainer;

        public AbletConsoleWindowView()
        {
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath));
            foreach (var reporter in BuildReporterRegistry.Instance.All())
            {
                if (reporter.StyleSheet)
                {
                    styleSheets.Add(reporter.StyleSheet);
                }
            }
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            container.Localize<AbletConsoleWindowView>();
            container.style.flexGrow = 1;
            BuildReporterList = container.Q<BuildReporterListView>("buildReporterListView");
            EntrypointList = container.Q<ListView>("entrypointList");
            ClearReportsButton = container.Q<ToolbarButton>("clearReports");
            SceneEntrypointsPopup = container.Q<GameObjectPopupField>("sceneEntrypointsPopup");
            EntrypointObjectField = container.Q<ObjectField>("entrypointObjectField");
            _buildReportsContainer = container.Q<VisualElement>("buildReportsContainer");
            hierarchy.Add(container);
        }

        public void DrawBuildReportLists(IEnumerable<SerializedBuildReportList> buildReportLists)
        {
            _buildReportsContainer.Clear();
            foreach (var buildReportList in buildReportLists)
            {
                var buildReportListView = new BuildReportListView();
                buildReportListView.Draw(buildReportList);
                _buildReportsContainer.Add(buildReportListView);
            }
        }

        public void DrawSettingsUI(VisualElement settingsUI)
        {
            _buildReportsContainer.Clear();
            _buildReportsContainer.Add(settingsUI);
        }
    }
}
