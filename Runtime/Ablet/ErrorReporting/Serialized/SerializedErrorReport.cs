using System;
using System.Collections.Generic;
using Ablet.API;

namespace Ablet.ErrorReporting.Serialized
{
    [Serializable]
    public class SerializedErrorReport : IAbletSerializedBuildReportPayload
    {
        public List<SerializedErrorLog> log = new List<SerializedErrorLog>();
    }

    [Serializable]
    public class SerializedErrorLog
    {
        public ErrorKind kind;
        public string message = "";
        public SerializedObjectChain[] interests = { };
        public SerializedExceptionLog exception = new SerializedExceptionLog();
    }

    [Serializable]
    public class SerializedExceptionLog
    {
        public string message = "";
        public string stacktrace = "";
    }
}
