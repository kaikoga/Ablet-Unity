using System;
using Ablet.Repositories;
using Silksprite.Loch.UIElements.Tools;
using UnityEditor;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    class AbletSettingsWindowView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/AbletSettingsWindowView.uxml";
        const string UssPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/Ablet.uss";

        public readonly Label IsCompilingLabel;
        public readonly VisualElement Container;

        public readonly VisualElement PresetsContainer;
        public readonly Button PresetPreferAbletButton;
        public readonly Button PresetPreferNdmfButton;
        
        public readonly Toggle AbletPreferNdmfToggle;
        public readonly Toggle ApplyOnPlayToggle;
        public readonly Toggle ApplyOnPlatformBuildToggle;
        public readonly Toggle AutoOpenConsoleWindowToggle;

        public readonly Toggle AbletOnNdmfToggle;
        public readonly VisualElement NdmfOnAbletContainer;
        public readonly Toggle NdmfOnAbletToggle;
        public readonly Label NdmfOnAbletDisabledLabel;
        public readonly VisualElement PreferAbletContainer;
        public readonly Toggle PreferAbletToggle;
        public readonly VisualElement SyncTranslationContainer;
        public readonly Toggle SyncTranslationToggle;

        public readonly Toggle EnhanceInplacePreviewToggle;

        public event Action? OnEditorSettingsChanged;

        public AbletSettingsWindowView(Action imgui)
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
            container.Localize<AbletSettingsWindowView>();
            container.Q<IMGUIContainer>("imgui").onGUIHandler = imgui;
            
            IsCompilingLabel = container.Q<Label>("isCompilingLabel");
            Container = container.Q<ScrollView>("container");

            PresetsContainer = container.Q<VisualElement>("presetsContainer");
            PresetPreferAbletButton = container.Q<Button>("presetPreferAbletButton");
            PresetPreferNdmfButton = container.Q<Button>("presetPreferNdmfButton");

            AbletPreferNdmfToggle = container.Q<Toggle>("abletPreferNdmfToggle");
            ApplyOnPlayToggle = container.Q<Toggle>("applyOnPlayToggle");
            ApplyOnPlatformBuildToggle = container.Q<Toggle>("applyOnPlatformBuildToggle");
            AutoOpenConsoleWindowToggle = container.Q<Toggle>("autoOpenConsoleWindowToggle");

            AbletOnNdmfToggle = container.Q<Toggle>("abletOnNdmfToggle");
            NdmfOnAbletContainer = container.Q<VisualElement>("ndmfOnAbletContainer");
            NdmfOnAbletToggle = container.Q<Toggle>("ndmfOnAbletToggle");
            NdmfOnAbletDisabledLabel = container.Q<Label>("ndmfOnAbletDisabledLabel");
            PreferAbletContainer = container.Q<VisualElement>("preferAbletContainer");
            PreferAbletToggle = container.Q<Toggle>("preferAbletToggle");
            SyncTranslationContainer = container.Q<VisualElement>("syncTranslationContainer");
            SyncTranslationToggle = container.Q<Toggle>("syncTranslationToggle");

            EnhanceInplacePreviewToggle = container.Q<Toggle>("enhanceInplacePreviewToggle");
            
            AbletPreferNdmfToggle.RegisterValueChangedCallback(evt => OnEditorSettingsChanged?.Invoke());
            ApplyOnPlayToggle.RegisterValueChangedCallback(evt => OnEditorSettingsChanged?.Invoke());
            ApplyOnPlatformBuildToggle.RegisterValueChangedCallback(evt => OnEditorSettingsChanged?.Invoke());
            AutoOpenConsoleWindowToggle.RegisterValueChangedCallback(evt => OnEditorSettingsChanged?.Invoke());
            AbletOnNdmfToggle.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue) NdmfOnAbletToggle.SetValueWithoutNotify(false);
                OnEditorSettingsChanged?.Invoke();
            });
            NdmfOnAbletToggle.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue) AbletOnNdmfToggle.SetValueWithoutNotify(false);
                OnEditorSettingsChanged?.Invoke();
            });
            PreferAbletToggle.RegisterValueChangedCallback(evt => OnEditorSettingsChanged?.Invoke());
            hierarchy.Add(container);
        }

        public void Draw(EditorSettingsValue settings, bool ndmf)
        {
            if (settings.AbletPreferNdmf)
            {
                AddToClassList("abletPreferNdmf");
            }
            else
            {
                RemoveFromClassList("abletPreferNdmf");
            }
            if (ndmf)
            {
                AddToClassList("ndmfVisible");
            }
            else
            {
                RemoveFromClassList("ndmfVisible");
            }
            AbletPreferNdmfToggle.SetValueWithoutNotify(settings.AbletPreferNdmf);
            ApplyOnPlayToggle.SetValueWithoutNotify(settings.ApplyOnPlay);
            ApplyOnPlatformBuildToggle.SetValueWithoutNotify(settings.ApplyOnPlatformBuild);
            AutoOpenConsoleWindowToggle.SetValueWithoutNotify(settings.AutoOpenConsoleWindow);
            AbletOnNdmfToggle.SetValueWithoutNotify(settings.NdmfInteropMode == NdmfInteropMode.AbletOnNdmf);
            NdmfOnAbletToggle.SetValueWithoutNotify(settings.NdmfInteropMode == NdmfInteropMode.NdmfOnAblet);
            PreferAbletToggle.SetValueWithoutNotify(settings.PreferAblet);
        }
    }
}
