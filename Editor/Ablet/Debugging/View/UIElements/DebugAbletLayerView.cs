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
            string FormatLayerPriority(int value) =>
                value switch
                {
                    int.MinValue => "min",
                    int.MaxValue => "max",
                    _ => value.ToString()
                };
            var sb = new StringBuilder();
            sb.Append(FormatLayerPriority(layer.LayerPriority));
            sb.Append(",");
            sb.Append(FormatLayerPriority(layer.InnerPriority));
            sb.Append(":");
            sb.Append(layer.Id + ": " + layer.DisplayName);
            return sb.ToString();
        }

    }
}
