using System;
using Ablet.ErrorReporting;
using Ablet.ErrorReporting.Serialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Builtin.ErrorReporting
{
    class ErrorLogView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Builtin/ErrorReporting/Uxml/ErrorLogView.uxml";

        readonly VisualElement _contextObjectsContainer;
        readonly TextField _messageText;
        
        public ErrorLogView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            _contextObjectsContainer = container.Q<VisualElement>("contextObjectsContainer");
            _messageText = container.Q<TextField>("messageText");
            hierarchy.Add(container);
        }

        public void Draw(SerializedErrorLog errorLog, GameObject? entrypointObject)
        {
            _contextObjectsContainer.Clear();
            foreach (var interest in errorLog.interests)
            {
                var interestView = new ObjectChainView();
                interestView.Draw(interest, entrypointObject);
                _contextObjectsContainer.Add(interestView);
            }
            AddToClassList(errorLog.kind.ToString());
            switch (errorLog.kind)
            {
                case ErrorKind.Error:
                case ErrorKind.Warning:
                case ErrorKind.Information:
                    _messageText.value = errorLog.message;
                    break;
                case ErrorKind.Exception:                
                    _messageText.value = $"{errorLog.exception.message}\n{errorLog.exception.stacktrace}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
