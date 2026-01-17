using System;
using Ablet.API;
using Ablet.EditorAPI.V1;
using Ablet.EditorAPI.V1.Attributes;
using Ablet.ErrorReporting.Serialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Builtin.ErrorReporting
{
    [AbletBuildReporter]
    class ErrorReporter : IAbletBuildReporter
    {
        const string UssPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Builtin/ErrorReporting/Uxml/ErrorLogView.uss";

        string IAbletDefinition.Id => BuiltinReporterIds.ErrorReporter;
        string IAbletDefinition.DisplayName => "Error Reports";
        
        Type IAbletBuildReporter.ForType => typeof(SerializedErrorReport);

        StyleSheet? IAbletBuildReporter.StyleSheet => AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);

        VisualElement IAbletBuildReporter.Render(IAbletSerializedBuildReportPayload payload, GameObject? entrypointObject)
        {
            var errorReportView = new ErrorReportView();
            errorReportView.Draw((SerializedErrorReport)payload, entrypointObject);
            return errorReportView;
        }
    }

}
