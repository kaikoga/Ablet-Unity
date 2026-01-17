using System;

namespace Ablet.ErrorReporting.Serialized
{
    [Serializable]
    public class SerializedObjectChain
    {
        public SerializedObjectReference source = new SerializedObjectReference();
        public SerializedObjectChainElement[] elements = { };
    }

    [Serializable]
    public class SerializedObjectChainElement
    {
        public SerializedObjectReference obj;
        public string layerId;

        public SerializedObjectChainElement(SerializedObjectReference obj, string layerId)
        {
            this.obj = obj;
            this.layerId = layerId;
        }
    }
}
