using UnityEngine;

using UnityEngine.UIElements;

#if !UNITY_2022_3_OR_NEWER
using UnityEditor.UIElements;
#endif

namespace Ablet.View.UIElements
{
    sealed class GameObjectPopupField : Silksprite.Loch.UIElements.LEditor.PopupField<GameObject>
    {
        public GameObjectPopupField()
        {
            formatListItemCallback = gameObject => gameObject ? gameObject.name : "";
            formatSelectedValueCallback = gameObject => gameObject ? gameObject.name : "";
        }

        public new class UxmlFactory : UxmlFactory<GameObjectPopupField, UxmlTraits>
        {
        }
    }
}
