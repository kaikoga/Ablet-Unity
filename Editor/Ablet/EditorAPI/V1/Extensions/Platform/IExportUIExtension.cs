using Ablet.API.V1;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.EditorAPI.V1.Extensions.Platform
{
    public interface IExportUIExtension : IAbletExtension
    {
        VisualElement RenderExportUI(GameObject entrypointObject);
    }
}
