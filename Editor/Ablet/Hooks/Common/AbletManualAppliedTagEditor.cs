using Ablet.Registries;
using UnityEditor;
using static Ablet.Loch.Tools.AbletLochTool;

namespace Ablet.Hooks.Common
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AbletManualAppliedTag))]
    class AbletManualAppliedTagEditor : Editor
    {
        SerializedProperty _platformId = null!;
        SerializedProperty _subplatformId = null!;

        void OnEnable()
        {
            _platformId = serializedObject.FindProperty(nameof(AbletManualAppliedTag.platformId));
            _subplatformId = serializedObject.FindProperty(nameof(AbletManualAppliedTag.subplatformId));
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.showMixedValue = _platformId.hasMultipleDifferentValues;
                var platformId = _platformId.stringValue;
                var platformDisplayName = PlatformRegistry.Instance.TryGetById(platformId, out var platform) ? platform.DisplayName : platformId;
                EditorGUILayout.TextField(Tr("AbletManualAppliedTag::Platform"), platformDisplayName);
                EditorGUI.showMixedValue = _platformId.hasMultipleDifferentValues;
                var subplatformId = _subplatformId.stringValue;
                var subplatformDisplayName = SubplatformRegistry.Instance.TryGetById(subplatformId, out var subplatform) ? subplatform.DisplayName : subplatformId;
                EditorGUILayout.TextField(Tr("AbletManualAppliedTag::Subplatform"), subplatformDisplayName);
                EditorGUI.showMixedValue = false;
            }
        }
    }
}
