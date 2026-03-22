using Ablet.ErrorReporting.Serialized;
using Ablet.Models;

namespace Ablet.ErrorReporting.Ephemeral
{
    class ObjectChainElement
    {
        public readonly SerializedObjectReference Obj;
        public readonly AbletLayer? Layer;

        public ObjectChainElement(SerializedObjectReference obj, AbletLayer? layer)
        {
            this.Obj = obj;
            this.Layer = layer;
        }
    }
}
