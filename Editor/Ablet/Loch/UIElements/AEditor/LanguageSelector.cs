using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseLanguageSelector = Silksprite.Loch.UIElements.LEditor.LanguageSelector;
#else
using BaseLanguageSelector = UnityEngine.UIElements.LanguageSelector;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class LanguageSelector : BaseLanguageSelector
    {
        public LanguageSelector() { }
        public new class UxmlFactory : UxmlFactory<LanguageSelector, UxmlTraits> {}
        public new class UxmlTraits : BaseLanguageSelector.UxmlTraits { }
    }
}
