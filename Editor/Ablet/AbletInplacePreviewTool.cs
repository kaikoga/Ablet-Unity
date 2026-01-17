using Ablet.Repositories;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Ablet
{
    [EditorTool("Ablet Inplace Preview", typeof(Transform))]
    class AbletInplacePreviewTool : EditorTool
    {
        GUIContent? _offIcon;
        GUIContent? _onIcon;
        public override GUIContent? toolbarIcon => EditorStateRepository.Instance.IsEnhancedInplacePreview ? _onIcon : _offIcon;

        void OnEnable()
        {
            _offIcon ??= new GUIContent(AssetDatabase.LoadAssetAtPath<Texture>("Packages/net.kaikoga.ablet/Icons/AbletIconOff.png"));
            _onIcon ??= new GUIContent(AssetDatabase.LoadAssetAtPath<Texture>("Packages/net.kaikoga.ablet/Icons/AbletIconOn.png"));
            ToolManager.activeToolChanged += OnActiveToolChanged;
        }

        void OnDisable()
        {
            ToolManager.activeToolChanged -= OnActiveToolChanged;
        }

        void OnActiveToolChanged()
        {
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (ToolManager.IsActiveTool(this))
            {
                EditorStateRepository.Instance.IsEnhancedInplacePreview = !EditorStateRepository.Instance.IsEnhancedInplacePreview;
                ToolManager.RestorePreviousTool();
#if UNITY_2022_3_OR_NEWER
                ToolManager.RefreshAvailableTools();
#endif
            }
        }
    }
}
