using System;

namespace Ablet.Models.Serialized
{
    [Serializable]
    public class SerializedLayerActivity
    {
        public string layerId;
        public long ticks;

        public SerializedLayerActivity(string layerId, long ticks)
        {
            this.layerId = layerId;
            this.ticks = ticks;
        }
    }
}
