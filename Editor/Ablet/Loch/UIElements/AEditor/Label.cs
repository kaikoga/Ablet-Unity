using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseLabel = Silksprite.Loch.UIElements.Label;
#else
using BaseLabel = UnityEngine.UIElements.Label;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class Label : BaseLabel
    {
        public Label() { }
        public new class UxmlFactory : UxmlFactory<Label, UxmlTraits> {}
        public new class UxmlTraits : BaseLabel.UxmlTraits { }
    }
}
