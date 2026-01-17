using System;
using Ablet.API;
using Ablet.Builtin;
using Ablet.EditorAPI.V1;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.View
{
    class MissingReporter : IAbletBuildReporter
    {
        string IAbletDefinition.Id => BuiltinReporterIds.Missing;
        string IAbletDefinition.DisplayName => "Missing GUI";

        public static AbletBuildReporter Instance => new AbletBuildReporter(new MissingReporter());

        MissingReporter() { }

        Type IAbletBuildReporter.ForType => typeof(object);
        StyleSheet? IAbletBuildReporter.StyleSheet => null;

        VisualElement IAbletBuildReporter.Render(IAbletSerializedBuildReportPayload payload, GameObject? entrypointObject)
        {
            return new Label($"{payload.GetType()} has no GUI");
        }
    }
}
