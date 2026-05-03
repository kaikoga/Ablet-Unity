using UnityEngine.UIElements;

#if ABLET_LOCH
using BaseUIElements = Silksprite.Loch.UIElements.LEditor;
#elif UNITY_2022_3_OR_NEWER
using BaseUIElements = UnityEngine.UIElements;
#else
using BaseUIElements = UnityEditor.UIElements;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class PopupField<T> : BaseUIElements.PopupField<T>
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<PopupField<T>, UxmlTraits> {}
        public new class UxmlTraits : BaseUIElements.PopupField<T>.UxmlTraits { }
#endif
    }
}
