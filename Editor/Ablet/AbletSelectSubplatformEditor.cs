using System;
using System.Linq;
using Ablet.Registries;
using UnityEditor;
using static Ablet.Loch.Tools.AbletLochTool;

namespace Ablet
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AbletSelectSubplatform))]
    class AbletSelectSubplatformEditor : Editor
    {
        AbletSelectSubplatform[] _abletSelectSubplatforms = null!;
        SerializedProperty _subplatformId = null!;
        string[] _subplatformIds = null!;
        string[] _subplatformDisplayNames = null!;
        readonly string[] _none = { "None" };
        readonly string[] _blank = { "" };

        void OnEnable()
        {
            _abletSelectSubplatforms = targets.OfType<AbletSelectSubplatform>().ToArray();
            _subplatformId = serializedObject.FindProperty(nameof(AbletSelectSubplatform.subplatformId));
            var allSubplatforms = SubplatformRegistry.Instance.All().Where(subplatform => subplatform.IsAvailable).ToArray();
            _subplatformIds = _blank.Concat(allSubplatforms.Select(x => x.Id)).ToArray();
            _subplatformDisplayNames = _none.Concat(allSubplatforms.Select(subplatform => subplatform.DisplayName)).ToArray();
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                var platformDisplayNames = _abletSelectSubplatforms
                    .Select(sf => sf.Subplatform?.Platform.DisplayName)
                    .OfType<string>()
                    .Distinct()
                    .ToArray();
                EditorGUI.showMixedValue = platformDisplayNames.Length > 1;
                EditorGUILayout.TextField(Tr("AbletSelectSubplatform.Platform"), platformDisplayNames.FirstOrDefault() ?? "");
            }
            EditorGUI.showMixedValue = _subplatformId.hasMultipleDifferentValues;
            var index = Array.IndexOf(_subplatformIds, _subplatformId.stringValue);
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup(Tr("AbletSelectSubplatform.Subplatform"), index, _subplatformDisplayNames);
            if (EditorGUI.EndChangeCheck())
            {
                _subplatformId.stringValue = _subplatformIds[index];
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.showMixedValue = false;
        }
    }
}
