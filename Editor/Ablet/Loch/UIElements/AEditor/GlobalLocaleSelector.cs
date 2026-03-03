using UnityEngine.UIElements;

#if ABLET_LOCH
using BaseGlobalLocaleSelector = Silksprite.Loch.UIElements.LEditor.GlobalLocaleSelector;
#else
using BaseGlobalLocaleSelector = UnityEngine.UIElements.VisualElement;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class GlobalLocaleSelector : BaseGlobalLocaleSelector
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !ABLET_LOCH

        public GlobalLocaleSelector()
        {
#if ABLET_NDMF
            hierarchy.Add(new nadena.dev.ndmf.ui.LanguageSwitcher());
#endif
        }

#endif
        public new class UxmlFactory : UxmlFactory<GlobalLocaleSelector, UxmlTraits> {}
        public new class UxmlTraits : BaseGlobalLocaleSelector.UxmlTraits { }
    }
}
