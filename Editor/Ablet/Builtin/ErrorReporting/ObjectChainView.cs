using System.Linq;
using Ablet.ErrorReporting.Serialized;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Builtin.ErrorReporting
{
    class ObjectChainView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Builtin/ErrorReporting/Uxml/ObjectChainView.uxml";

        readonly ObjectField _objectField;
        readonly VisualElement _elementContainer;
        
        public ObjectChainView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            _objectField = container.Q<ObjectField>("objectField");
            _elementContainer = container.Q<VisualElement>("elementContainer");
            hierarchy.Add(container);
        }

        public void Draw(SerializedObjectChain chain, GameObject? entrypointObject)
        {
            var resolvedChain = chain.elements
                .Select(element =>
                {
                    if (entrypointObject == null || !element.obj.TryResolveRelative(entrypointObject, out var obj))
                    {
                        obj = null;
                    }
                    return (element, obj);
                })
                .ToArray();

            var sceneObject = resolvedChain.Select(element => element.obj).FirstOrDefault(obj => obj);

            _objectField.SetValueWithoutNotify(sceneObject);
            _objectField.SetEnabled(false);
            
            _elementContainer.Clear();
            foreach (var resolvedElement in resolvedChain)
            {
                var elementView = new ObjectChainElementView();
                elementView.Draw(resolvedElement.element, resolvedElement.obj);
                _elementContainer.Add(elementView);
            }
        }
    }
}
