using Ablet.API.V1;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.EditorAPI.V1.Extensions.Subplatform
{
    public interface ISubExportUIExtension : IAbletExtension
    {
        VisualElement RenderSubExportUI(GameObject entrypointObject);
    }
}
