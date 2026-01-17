using Ablet.API.V1;
using UnityEngine.UIElements;

namespace Ablet.EditorAPI.V1.Extensions.BuildReporter
{
    public interface ISettingsUIExtension : IAbletExtension
    {
        VisualElement RenderSettingsUI();
    }
}
