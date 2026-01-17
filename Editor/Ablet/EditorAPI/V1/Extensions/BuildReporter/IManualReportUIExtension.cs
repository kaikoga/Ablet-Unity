using Ablet.API.V1;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.EditorAPI.V1.Extensions.BuildReporter
{
    public interface IManualReportUIExtension : IAbletExtension
    {
        VisualElement RenderManualReportUI(GameObject entrypointObject);
    }
}
