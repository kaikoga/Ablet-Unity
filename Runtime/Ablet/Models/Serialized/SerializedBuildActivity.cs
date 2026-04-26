using System;
using System.Collections.Generic;

namespace Ablet.Models.Serialized
{
    [Serializable]
    public class SerializedBuildActivity
    {
        public string catalystId;
        public List<SerializedLayerActivity> layers = new List<SerializedLayerActivity>();
        public SerializedBuildActivity(string catalystId)
        {
            this.catalystId = catalystId;
        }
    }
}
