using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseHeading = Silksprite.Loch.UIElements.Heading;
#else
using BaseHeading = UnityEngine.UIElements.Label;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class Heading : BaseHeading
    {
        public Heading() { }
        public new class UxmlFactory : UxmlFactory<Heading, UxmlTraits> {}
        public new class UxmlTraits : BaseHeading.UxmlTraits { }
    }
}
