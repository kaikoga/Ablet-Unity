using System;
using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseButton = Silksprite.Loch.UIElements.Button;
#else
using BaseButton = UnityEngine.UIElements.Button;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class Button : BaseButton
    {
        public Button() { }
        public Button(Action clickEvent) : base(clickEvent) { }
        public new class UxmlFactory : UxmlFactory<Button, UxmlTraits> {}
        public new class UxmlTraits : BaseButton.UxmlTraits { }
    }
}
