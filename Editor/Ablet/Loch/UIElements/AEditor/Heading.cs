using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseHeading = Silksprite.Loch.UIElements.Heading;
#else
using BaseHeading = UnityEngine.UIElements.Label;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class Heading : BaseHeading
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<Heading, UxmlTraits> {}
        public new class UxmlTraits : BaseHeading.UxmlTraits { }
#endif
    }
}
