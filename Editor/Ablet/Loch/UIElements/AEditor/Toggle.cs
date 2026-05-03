using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseToggle = Silksprite.Loch.UIElements.Toggle;
#else
using BaseToggle = UnityEngine.UIElements.Toggle;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class Toggle : BaseToggle
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<Toggle, UxmlTraits> {}
        public new class UxmlTraits : BaseToggle.UxmlTraits { }
#endif
    }
}
