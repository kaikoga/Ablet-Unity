using System;
using Ablet.Repositories;
using Ablet.View.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if ABLET_NDMF
using Ablet.Ndmf;
#endif

namespace Ablet.View.Windows
{
    class AbletSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Ablet/Ablet Settings Window", false, 80)]
        static void ShowWindow()
        {
            var window = GetWindow<AbletSettingsWindow>() ?? CreateInstance<AbletSettingsWindow>();
            window.Show();
        }

        AbletSettingsWindowView? _view;

        void CreateGUI()
        {
            titleContent = new GUIContent("Ablet Settings Window");
            minSize = new Vector2(600f, 400f);
            _view = new AbletSettingsWindowView(OnInnerGUI);
            
#if ABLET_NDMF
            _view.PresetPreferAbletButton.clicked += () =>
            {
                EditorSettingsRepository.Instance.Save(EditorSettingsValue.DefaultPreferAblet);
                NdmfConfigUpdater.UpdateNdmfConfig();
            };
            _view.PresetPreferNdmfButton.clicked += () =>
            {
                EditorSettingsRepository.Instance.Save(EditorSettingsValue.DefaultPreferNdmf);
                NdmfConfigUpdater.RevertNdmfConfig();
            };
#endif
            _view.OnEditorSettingsChanged += () =>
            {
                var settings = EditorSettingsRepository.Instance.Value;
                settings.AbletPreferNdmf = _view.AbletPreferNdmfToggle.value;
                settings.ApplyOnPlay = _view.ApplyOnPlayToggle.value;
                settings.ApplyOnPlatformBuild = _view.ApplyOnPlatformBuildToggle.value;
                settings.AutoOpenConsoleWindow = _view.AutoOpenConsoleWindowToggle.value;
                settings.NdmfInteropMode = _view.AbletOnNdmfToggle.value ? NdmfInteropMode.AbletOnNdmf
                    : _view.NdmfOnAbletToggle.value ? NdmfInteropMode.NdmfOnAblet
                    : NdmfInteropMode.None;
                settings.PreferAblet = _view.PreferAbletToggle.value;
                EditorSettingsRepository.Instance.Save();
            };
            EditorSettingsRepository.Instance.OnChanged += Redraw;
            Redraw();

#if ABLET_NDMF && ABLET_LOCH
            _view.SyncTranslationToggle.RegisterValueChangedCallback(evt =>
                Silksprite.Loch.Core.NdmfSyncSettingRepository.Instance.IsNdmfSyncEnabled = evt.newValue);
            Silksprite.Loch.Core.LochRepository.Instance.OnLanguageChanged += RedrawNdmfSync;
            RedrawNdmfSync();
#endif

            _view.EnhanceInplacePreviewToggle.RegisterValueChangedCallback(evt =>
                EditorStateRepository.Instance.IsEnhancedInplacePreview = evt.newValue);
            EditorStateRepository.Instance.OnChanged += RedrawEnhanceInplacePreview;
            RedrawEnhanceInplacePreview();

            rootVisualElement.Add(_view);
        }

        void OnDisable()
        {
            EditorSettingsRepository.Instance.OnChanged -= Redraw;
#if ABLET_NDMF && ABLET_LOCH
            Silksprite.Loch.Core.LochRepository.Instance.OnLanguageChanged -= RedrawNdmfSync;
#endif
            EditorStateRepository.Instance.OnChanged -= RedrawEnhanceInplacePreview;
        }

        void Redraw()
        {
            if (_view == null)
            {
                return;
            }
            var settings = EditorSettingsRepository.Instance.Value;
#if ABLET_NDMF
            _view.Draw(settings, true);
#else
            _view.Draw(settings, false);
#endif
        }

#if ABLET_NDMF && ABLET_LOCH
        void RedrawNdmfSync()
        {
            _view?.SyncTranslationToggle.SetValueWithoutNotify(Silksprite.Loch.Core.NdmfSyncSettingRepository.Instance.IsNdmfSyncEnabled);
        }
#endif

        void RedrawEnhanceInplacePreview()
        {
            _view?.EnhanceInplacePreviewToggle.SetValueWithoutNotify(EditorStateRepository.Instance.IsEnhancedInplacePreview);
        }

        void OnInnerGUI()
        {
            if (_view == null)
            {
                return;
            }
            var isCompiling = EditorApplication.isCompiling || EditorApplication.isUpdating;
            _view.IsCompilingLabel.style.display = isCompiling ? DisplayStyle.Flex : DisplayStyle.None;
            _view.Container.SetEnabled(!isCompiling);

            var isNdmfOnAbletAvailable = NdmfConfigAccess.IsNdmfOnAbletAvailable();
            _view.NdmfOnAbletContainer.SetEnabled(isNdmfOnAbletAvailable);
            _view.NdmfOnAbletDisabledLabel.style.display = isNdmfOnAbletAvailable ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
