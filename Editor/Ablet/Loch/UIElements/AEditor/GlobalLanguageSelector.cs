using UnityEngine.UIElements;

#if ABLET_LOCH
using BaseLanguageSelector = Silksprite.Loch.UIElements.LEditor.LanguageSelector;
#else
using BaseLanguageSelector = UnityEngine.UIElements.VisualElement;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class GlobalLanguageSelector : BaseLanguageSelector
    {
#if !ABLET_LOCH

        public GlobalLanguageSelector()
        {
#if ABLET_NDMF
            hierarchy.Add(new nadena.dev.ndmf.ui.LanguageSwitcher());
#endif
        }

#endif
        public new class UxmlFactory : UxmlFactory<GlobalLanguageSelector, UxmlTraits> {}
        public new class UxmlTraits : BaseLanguageSelector.UxmlTraits { }
    }
}
