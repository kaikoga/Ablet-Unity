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
        readonly Foldout _messageFoldout;

        string _message = "";
        string _expandedMessage = "";
        
        public ErrorLogView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            _contextObjectsContainer = container.Q<VisualElement>("contextObjectsContainer");
            _messageText = container.Q<TextField>("messageText");
            _messageFoldout = container.Q<Foldout>("messageFoldout"); 
            _messageFoldout.RegisterValueChangedCallback(evt => DrawMessage(evt.newValue));
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
                    _message = errorLog.text.message;
                    _expandedMessage = $"{errorLog.text.message}\n{errorLog.text.stacktrace}";
                    break;
                case ErrorKind.Exception:                
                    _message = $"{errorLog.exception.type}: {errorLog.exception.message}";
                    _expandedMessage = $"{errorLog.exception.type}\n{errorLog.exception.message}\n{errorLog.exception.stacktrace}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _messageFoldout.value = false;
            DrawMessage(false);
        }

        void DrawMessage(bool isExpanded)
        {
            _messageText.value = isExpanded ? _expandedMessage : _message;
        }
    }
}
