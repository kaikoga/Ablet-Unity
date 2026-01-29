using UnityEditor;
using UnityEngine;

namespace Ablet.DataObjects
{
    public abstract class SerializedReferencePathDrawer<T> : PropertyDrawer
    where T : Component
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, serializedProperty))
            {
                var serializedHasValue = serializedProperty.FindPropertyRelative("hasValue");
                var serializedRelativePath = serializedProperty.FindPropertyRelative("relativePath");
                if ((serializedProperty.serializedObject.targetObject as Component)?.transform is { } transform)
                {
                    var relativePathValue = serializedHasValue.boolValue ? serializedRelativePath.stringValue : "";
                    var obj = SerializedReferencePath.ResolveNow<T>(transform, relativePathValue);
                    using var change = new EditorGUI.ChangeCheckScope();
                    obj = EditorGUI.ObjectField(position, label, obj, typeof(T), true) as T;
                    if (change.changed)
                    {
                        var newRelativePathValue = obj != null ? SerializedReferencePath.RelativePath(transform, obj) : null;
                        serializedRelativePath.stringValue = newRelativePathValue ?? "";
                        serializedHasValue.boolValue = newRelativePathValue != null;
                    }
                }
                else
                {
                    using var _ = new EditorGUI.DisabledScope(true);
                    EditorGUI.TextField(position, serializedRelativePath.displayName, serializedRelativePath.stringValue);
                }
            }
        }
    }
    
    [CustomPropertyDrawer(typeof(SerializedTransformPath))]
    public class SerializedTransformPathDrawer : SerializedReferencePathDrawer<Transform> { }
}
