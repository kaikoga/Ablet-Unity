using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseToggle = Silksprite.Loch.UIElements.Toggle;
#else
using BaseToggle = UnityEngine.UIElements.Toggle;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class Toggle : BaseToggle
    {
        public Toggle() { }
        public new class UxmlFactory : UxmlFactory<Toggle, UxmlTraits> {}
        public new class UxmlTraits : BaseToggle.UxmlTraits { }
    }
}
