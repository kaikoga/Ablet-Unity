using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseToolbarButton = Silksprite.Loch.UIElements.LEditor.ToolbarButton;
#else
using BaseToolbarButton = UnityEditor.UIElements.ToolbarButton;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class ToolbarButton : BaseToolbarButton
    {
        public new class UxmlFactory : UxmlFactory<ToolbarButton, UxmlTraits> {}
        public new class UxmlTraits : BaseToolbarButton.UxmlTraits { }
    }
}
