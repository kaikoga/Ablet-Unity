using System.Linq;
using Ablet.EditorAPI.V1.Extensions.BuildReporter;
using Ablet.EditorAPI.V1.Extensions.Platform;
using Ablet.Models.Extensions;
using Ablet.Models.Serialized;
using Ablet.Registries;
using Ablet.Repositories;
using Silksprite.Loch.UIElements.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    class BuildReportListView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/BuildReportListView.uxml";

        readonly Label _entrypointDisplayNameText;
        readonly Label _entrypointScenePathText;
        readonly ObjectField _entrypointSceneAssetField;
        readonly VisualElement _entrypointObjectContainer;
        readonly VisualElement _noEntrypointObjectContainer;
        readonly ObjectField _entrypointObjectField;

        readonly VisualElement _resultContainer;
        readonly VisualElement _exportUIContainer;
        readonly VisualElement _actionsContainer;
        readonly VisualElement _noActionsContainer;

        SerializedEntrypointReference? _entrypointRef;

        public BuildReportListView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            container.Localize<BuildReportListView>();

            _entrypointDisplayNameText = container.Q<Label>("entrypointDisplayNameText");
            _entrypointScenePathText = container.Q<Label>("entrypointScenePathText");
            _entrypointSceneAssetField = container.Q<ObjectField>("entrypointSceneAssetField");
            _entrypointObjectContainer = container.Q<VisualElement>("entrypointObjectContainer");
            _noEntrypointObjectContainer = container.Q<VisualElement>("noEntrypointObjectContainer");
            _entrypointObjectField = container.Q<ObjectField>("entrypointObjectField");
            var openSceneButton = container.Q<Button>("openSceneButton");
            openSceneButton.clickable.clicked += OnOpenScene;
            var openSceneAdditiveButton = container.Q<Button>("openSceneAdditiveButton");
            openSceneAdditiveButton.clickable.clicked += OnOpenSceneAdditive;
            
            _resultContainer = container.Q<VisualElement>("resultContainer");
            _actionsContainer = container.Q<VisualElement>("actionsContainer");
            _noActionsContainer = container.Q<VisualElement>("noActionsContainer");
            _exportUIContainer = container.Q<VisualElement>("exportUIContainer");

            var manualApplyButton = container.Q<Button>("manualApplyButton");
            manualApplyButton.clickable.clicked += OnManualApply;
            hierarchy.Add(container);
        }

        public void Draw(SerializedBuildReportList buildReportList)
        {
            _entrypointRef = buildReportList.entrypointRef;
            _entrypointDisplayNameText.text = _entrypointRef.DisplayName;
            _entrypointSceneAssetField.value = AssetDatabase.LoadAssetAtPath<SceneAsset>(_entrypointRef.ScenePath);
            _entrypointSceneAssetField.SetEnabled(false);
            _entrypointScenePathText.text = _entrypointRef.ScenePath;
            _entrypointScenePathText.style.display = BuildReportRepository.Instance.IsMultiScene ? DisplayStyle.Flex : DisplayStyle.None;

            if (_entrypointRef.TryResolve(out var entrypointObject))
            {
                _entrypointObjectContainer.style.display = DisplayStyle.Flex;
                _noEntrypointObjectContainer.style.display = DisplayStyle.None;
                _entrypointObjectField.value = entrypointObject;
                _entrypointObjectField.SetEnabled(false);
            }
            else
            {
                _entrypointObjectContainer.style.display = DisplayStyle.None;
                _noEntrypointObjectContainer.style.display = DisplayStyle.Flex;
            }
            
            _resultContainer.Clear();
            foreach (var layerReport in buildReportList.buildReports.GroupBy(buildReport => buildReport.layerId))
            {
                var buildReportView = new LayerReportView();
                buildReportView.Draw(layerReport.Key, layerReport, entrypointObject);
                _resultContainer.Add(buildReportView);
            }

            _exportUIContainer.Clear();
            if (entrypointObject != null)
            {
                _actionsContainer.style.display = DisplayStyle.Flex;
                _noActionsContainer.style.display = DisplayStyle.None;
                
                if (PlatformRegistry.Instance.TryGuessPlatform(entrypointObject, out var platform)
                    && platform.TryGetExtensionDef<IExportUIExtension>(out var exportUI))
                {
                    _exportUIContainer.Add(exportUI.RenderExportUI(entrypointObject));
                }

                foreach (var reporter in BuildReporterRegistry.Instance.All())
                {
                    if (reporter.TryGetExtensionDef<IManualReportUIExtension>(out var manualReportUI))
                    {
                        _exportUIContainer.Add(manualReportUI.RenderManualReportUI(entrypointObject));
                    }
                }
            }
            else
            {
                _actionsContainer.style.display = DisplayStyle.None;
                _noActionsContainer.style.display = DisplayStyle.Flex;
            }
        }

        void OnOpenScene()
        {
            if (_entrypointRef?.ScenePath is {} scenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                }
            }
        }

        void OnOpenSceneAdditive()
        {
            if (_entrypointRef?.ScenePath is {} scenePath)
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        void OnManualApply()
        {
            if (_entrypointRef != null && _entrypointRef.TryResolve(out var resolved))
            {
                AbletFacade.ManualApplyToGameObject(resolved);
            }
        }
    }
}
