using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

#if ABLET_NDMF
using Ablet.Ndmf;
#endif

namespace Ablet.View.Windows
{
    class AbletSettingsWindow : EditorWindow
    {
        Vector2 _scrollPosition = new Vector2(0, 0);

        void OnGUI()
        {
            void HelpLabel(string message)
            {
                GUILayout.Label(message.Replace(" ", " "), new GUIStyle{wordWrap = true});
            }

            var isCompiling = EditorApplication.isCompiling || EditorApplication.isUpdating;
            if (isCompiling)
            {
                HelpLabel("設定を更新中なのでしばらく待ってね");
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            using var _ = new EditorGUI.DisabledScope(isCompiling);

            var settings = EditorSettingsRepository.Instance.Value;
#if ABLET_NDMF
            GUILayout.Label("一括設定", new GUIStyle { fontStyle = FontStyle.Bold });
            if (GUILayout.Button("Ablet 優先設定"))
            {
                EditorSettingsRepository.Instance.Save(EditorSettingsValue.DefaultPreferAblet);
                NdmfConfigUpdater.UpdateNdmfConfig();
            }
            HelpLabel("Abletをメインで利用します。\nNDMF専用プラグインをAbletの上で互換動作させます。\nハイブリッドプラグインはAbletプラグインとして動作します。\nビルドはAbletから行ってください。");
            if (GUILayout.Button("NDMF 優先設定"))
            {
                EditorSettingsRepository.Instance.Save(EditorSettingsValue.DefaultPreferNdmf);
                NdmfConfigUpdater.RevertNdmfConfig();
            }
            HelpLabel("NDMFをメインで利用します。\nAblet専用プラグインをNDMFの上で互換動作させます。\nハイブリッドプラグインはNDMFプラグインとして動作します。\nビルドはNDMFから行ってください。");
#endif
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                // ReSharper disable RedundantAssignment
                var abletPreferNdmf = settings.AbletPreferNdmf;
                var applyOnPlay = settings.ApplyOnPlay;
                var applyOnPlatformBuild = settings.ApplyOnPlatformBuild;
                var autoOpenConsoleWindow = settings.AutoOpenConsoleWindow;
                var abletOnNdmf = settings.IsAbletOnNdmf;
                var ndmfOnAblet = settings.IsNdmfOnAblet;
                var preferAblet = settings.PreferAblet;
                // ReSharper enable RedundantAssignment

#if ABLET_NDMF
                GUILayout.Label("全体設定", new GUIStyle { fontStyle = FontStyle.Bold });
                abletPreferNdmf = GUILayout.Toggle(abletPreferNdmf, "Ablet Prefer NDMF");
                HelpLabel("オンの場合、NDMFの機能を使用する設定になります。Abletの同等の機能は非表示になります。");
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
#endif

                GUILayout.Label("保存される設定", new GUIStyle { fontStyle = FontStyle.Bold });
                if (!abletPreferNdmf)
                {
                    applyOnPlay = GUILayout.Toggle(applyOnPlay, "Apply on Play");
                    HelpLabel("プレイモードに遷移した際、AbletのApply on Playを適用します。");
#if ABLET_NDMF
                    HelpLabel("オンの場合、NDMFのApply on Playは無効化されます。");
#endif
                    applyOnPlatformBuild = GUILayout.Toggle(applyOnPlatformBuild, "Apply on Platform Build");
                    HelpLabel("プラットフォームへのエクスポートを行う際、AbletのApply on Platform Buildを適用します。");
#if ABLET_NDMF
                    HelpLabel("オンの場合、NDMFのApply on Buildは無効化されます。");
#endif
                }
                autoOpenConsoleWindow = GUILayout.Toggle(autoOpenConsoleWindow, "Auto Open Console Window");
                HelpLabel("オンの場合、Abletのコンソールが更新された際に自動的に表示します。");
#if ABLET_NDMF
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                GUILayout.Label("保存される設定 (NDMF)", new GUIStyle { fontStyle = FontStyle.Bold });
                abletOnNdmf = GUILayout.Toggle(abletOnNdmf, "Ablet on NDMF");
                HelpLabel("NDMFからAblet専用プラグインを呼び出します。");

                if (!abletPreferNdmf)
                {
                    var isNdmfOnAbletAvailable = NdmfConfigAccess.IsNdmfOnAbletAvailable();
                    using (new EditorGUI.DisabledScope(!isNdmfOnAbletAvailable))
                    {
                        ndmfOnAblet = ndmfOnAblet && !abletOnNdmf;
                        ndmfOnAblet = GUILayout.Toggle(ndmfOnAblet, "NDMF on Ablet");
                        if (ndmfOnAblet) abletOnNdmf = false;
                        HelpLabel("AbletからNDMF専用プラグインを呼び出します。");
                    }
                    if (!isNdmfOnAbletAvailable)
                    {
                        HelpLabel("NDMFのApply on Playが有効なので、NDMF on Abletを有効にできません。");
                    }
                    preferAblet = GUILayout.Toggle(preferAblet, "Prefer Ablet");
                    HelpLabel("Ablet NDMFハイブリッドプラグインの動作を設定します。\nオンの場合、Abletプラグインとして動作します。\nオフの場合、NDMFプラグインとして動作します。");
                }
#endif
                if (change.changed)
                {
                    settings.AbletPreferNdmf = abletPreferNdmf;
                    settings.ApplyOnPlay = applyOnPlay;
                    settings.ApplyOnPlatformBuild = applyOnPlatformBuild;
                    settings.AutoOpenConsoleWindow = autoOpenConsoleWindow;
                    settings.NdmfInteropMode = abletOnNdmf ? NdmfInteropMode.AbletOnNdmf
                        : ndmfOnAblet ? NdmfInteropMode.NdmfOnAblet
                        : NdmfInteropMode.None;
                    settings.PreferAblet = preferAblet;
                    EditorSettingsRepository.Instance.Save();
                }
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                GUILayout.Label("一時的な設定", new GUIStyle { fontStyle = FontStyle.Bold });
                EditorStateRepository.Instance.IsEnhancedInplacePreview = GUILayout.Toggle(EditorStateRepository.Instance.IsEnhancedInplacePreview, "Enhance Inplace Preview");
                HelpLabel("AbletのInplace Previewを行う際、一部のAbletプラグインの編集を適用します。");
            }
            EditorGUILayout.EndScrollView();
        }

        [MenuItem("Tools/Ablet/Ablet Settings Window", false, 80)]
        static void ShowWindow()
        {
            var window = GetWindow<AbletSettingsWindow>() ?? CreateInstance<AbletSettingsWindow>();
            window.titleContent = new GUIContent("Ablet Settings Window");
            window.minSize = new Vector2(600f, 400f);
            window.Show();
        }
    }
}
