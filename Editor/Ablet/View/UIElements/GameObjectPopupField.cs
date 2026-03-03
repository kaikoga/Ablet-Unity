using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    sealed class GameObjectPopupField : Ablet.Loch.UIElements.AEditor.PopupField<GameObject>
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
