using System;
using System.Collections.Generic;
using Ablet.API;
using UnityEngine;

namespace Ablet.Models.Serialized
{
    [Serializable]
    public class SerializedBuildReportList
    {
        public SerializedEntrypointReference entrypointRef = new SerializedEntrypointReference("", "");
        public List<SerializedBuildReport> buildReports = new List<SerializedBuildReport>();
    }

    [Serializable]
    public class SerializedBuildReport
    {
        public SerializedEntrypointReference entrypointRef;
        public string layerId;
        [SerializeReference] public IAbletSerializedBuildReportPayload payload;

        public SerializedBuildReport(SerializedEntrypointReference entrypointRef, string layerId, IAbletSerializedBuildReportPayload payload)
        {
            this.entrypointRef = entrypointRef;
            this.layerId = layerId;
            this.payload = payload;
        }
    }
}
