using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseObjectField = Silksprite.Loch.UIElements.LEditor.ObjectField;
#else
using BaseObjectField = UnityEditor.UIElements.ObjectField;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
#if UNITY_2023_2_OR_NEWER
    [UxmlElement] public partial 
#else
    public
#endif
        class ObjectField : BaseObjectField
    {
#if !ABLET_LOCH
        public string? loc;
#endif

#if !UNITY_2023_2_OR_NEWER
        public new class UxmlFactory : UxmlFactory<ObjectField, UxmlTraits> {}
        public new class UxmlTraits : BaseObjectField.UxmlTraits { }
#endif
    }
}
