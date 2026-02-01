using System.Linq;
using System.Text;
using Ablet.Planning;
using UnityEngine.UIElements;

namespace Ablet.Debugging.View.UIElements
{
    public class DebugAbletPassView : VisualElement
    {
        readonly Label _label;

        public DebugAbletPassView()
        {
            _label = new Label();
            hierarchy.Add(_label);
        }

        public void Draw(AbletPass pass)
        {
            _label.text = Format(pass);
        }

        public static string Format(AbletPass pass)
        {
            var sb = new StringBuilder();
            string FormatLayerPriority(int value)
            {
                return value == int.MinValue ? "min" : value.ToString();
            }
            sb.AppendJoin("", Enumerable.Repeat("  ", pass.Depth));
            sb.Append(" ");
            sb.Append(pass.IsContainerPass ? "[" : "<");
            sb.Append(FormatLayerPriority(pass.Layer.LayerPriority));
            sb.Append(",");
            sb.Append(FormatLayerPriority(pass.Layer.InnerPriority));
            sb.Append(pass.IsContainerPass ? "]" : ">");
            sb.Append(pass.Layer.Id + ": " + pass.Layer.DisplayName);
            return sb.ToString();
        }

    }
}
