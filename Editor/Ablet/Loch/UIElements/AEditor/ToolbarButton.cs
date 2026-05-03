using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseToolbarButton = Silksprite.Loch.UIElements.LEditor.ToolbarButton;
#else
using BaseToolbarButton = UnityEditor.UIElements.ToolbarButton;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class ToolbarButton : BaseToolbarButton
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<ToolbarButton, UxmlTraits> {}
        public new class UxmlTraits : BaseToolbarButton.UxmlTraits { }
#endif
    }
}
