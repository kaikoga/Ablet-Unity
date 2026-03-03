using UnityEngine.UIElements;
#if ABLET_LOCH
using BaseObjectField = Silksprite.Loch.UIElements.LEditor.ObjectField;
#else
using BaseObjectField = UnityEditor.UIElements.ObjectField;
#endif

namespace Ablet.Loch.UIElements.AEditor
{
    public class ObjectField : BaseObjectField
    {
#if !ABLET_LOCH
        public string? loc;
#endif

        public new class UxmlFactory : UxmlFactory<ObjectField, UxmlTraits> {}
        public new class UxmlTraits : BaseObjectField.UxmlTraits { }
    }
}
