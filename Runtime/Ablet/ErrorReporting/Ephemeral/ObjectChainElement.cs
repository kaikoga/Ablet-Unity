using Ablet.ErrorReporting.Serialized;
using Ablet.Models;

namespace Ablet.ErrorReporting.Ephemeral
{
    class ObjectChainElement
    {
        public readonly SerializedObjectReference obj;
        public readonly AbletLayer? layer;

        public ObjectChainElement(SerializedObjectReference obj, AbletLayer? layer)
        {
            this.obj = obj;
            this.layer = layer;
        }
    }
}
