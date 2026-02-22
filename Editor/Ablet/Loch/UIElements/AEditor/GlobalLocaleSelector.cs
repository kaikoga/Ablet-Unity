using Silksprite.Loch.UIElements.LEditor;
using UnityEngine.UIElements;

#if ABLET_LOCH

#else
using BaseLanguageSelector = UnityEngine.UIElements.VisualElement;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class GlobalLocaleSelector : LocaleSelector
    {
#if !ABLET_LOCH

        public GlobalLanguageSelector()
        {
#if ABLET_NDMF
            hierarchy.Add(new nadena.dev.ndmf.ui.LanguageSwitcher());
#endif
        }

#endif
        public new class UxmlFactory : UxmlFactory<GlobalLocaleSelector, UxmlTraits> {}
        public new class UxmlTraits : LocaleSelector.UxmlTraits { }
    }
}
