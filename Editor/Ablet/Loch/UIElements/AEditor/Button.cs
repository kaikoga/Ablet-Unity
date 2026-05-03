using System;
using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseButton = Silksprite.Loch.UIElements.Button;
#else
using BaseButton = UnityEngine.UIElements.Button;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class Button : BaseButton
    {
#if !ABLET_LOCH
        public string? loc;
#endif

        public Button() { }
        public Button(Action clickEvent) : base(clickEvent) { }

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<Button, UxmlTraits> {}
        public new class UxmlTraits : BaseButton.UxmlTraits { }
#endif

    }
}
