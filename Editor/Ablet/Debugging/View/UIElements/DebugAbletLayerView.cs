using System.Text;
using Ablet.Models;
using UnityEngine.UIElements;

namespace Ablet.Debugging.View.UIElements
{
    public class DebugAbletLayerView : VisualElement
    {
        readonly Label _label;

        public DebugAbletLayerView()
        {
            _label = new Label();
            hierarchy.Add(_label);
        }

        public void Draw(AbletLayer layer)
        {
            _label.text = Format(layer);
        }

        static string Format(AbletLayer layer)
        {
            var sb = new StringBuilder();
            string FormatLayerPriority(int value)
            {
                return value == int.MinValue ? "min" : value.ToString();
            }
            sb.Append(FormatLayerPriority(layer.LayerPriority));
            sb.Append(",");
            sb.Append(FormatLayerPriority(layer.InnerPriority));
            sb.Append(":");
            sb.Append(layer.Id + ": " + layer.DisplayName);
            return sb.ToString();
        }

    }
}
