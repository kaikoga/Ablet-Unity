using System.Linq;
using Ablet.ErrorReporting.Serialized;
using Ablet.Registries;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.ErrorReporting
{
    class ObjectChainElementView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Builtin/ErrorReporting/Uxml/ObjectChainElementView.uxml";

        readonly Label _layerNameText;
        readonly Image _iconImage;
        readonly TextField _pathField;
        
        public ObjectChainElementView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            _layerNameText = container.Q<Label>("layerNameText");
            _iconImage = container.Q<Image>("iconImage");
            _pathField = container.Q<TextField>("pathField");
            hierarchy.Add(container);
        }

        public void Draw(SerializedObjectChainElement chainElement, Object? chainObject)
        {
            var layerName = LayerRegistry.Instance.ToDisplayName(chainElement.layerId);
            var icon = chainObject != null
                ? EditorGUIUtility.GetIconForObject(chainObject) ?? AssetPreview.GetMiniTypeThumbnail(chainObject.GetType())
                : null;
            var typeName = chainElement.obj.type switch
            {
                "" => "null",
                _ => chainElement.obj.type.Split(",", 2).First().Split(".").Last()
            };
            _layerNameText.text = layerName;
            _iconImage.image = icon;
            _pathField.value = $"{chainElement.obj.source}: {chainElement.obj.path} ({typeName})";
        }
    }
}
