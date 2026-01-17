using System;
using Ablet.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.EditorAPI.V1
{
    public interface IAbletBuildReporter : IAbletDefinition
    {
        Type ForType { get; }

        StyleSheet? StyleSheet { get; }
        VisualElement? Render(IAbletSerializedBuildReportPayload payload, GameObject? entrypointObject);
    }
}
