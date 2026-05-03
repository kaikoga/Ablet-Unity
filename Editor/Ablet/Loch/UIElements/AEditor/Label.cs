using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseLabel = Silksprite.Loch.UIElements.Label;
#else
using BaseLabel = UnityEngine.UIElements.Label;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class Label : BaseLabel
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<Label, UxmlTraits> {}
        public new class UxmlTraits : BaseLabel.UxmlTraits { }
#endif
    }
}
