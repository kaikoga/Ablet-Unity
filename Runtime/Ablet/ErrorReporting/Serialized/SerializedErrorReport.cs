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
        public SerializedObjectChain[] interests = { };
        public SerializedTextLog text = new SerializedTextLog();
        public SerializedExceptionLog exception = new SerializedExceptionLog();
    }

    [Serializable]
    public class SerializedTextLog
    {
        public string message = "";
        public string stacktrace = "";
    }

    [Serializable]
    public class SerializedExceptionLog
    {
        public string type = "";
        public string message = "";
        public string stacktrace = "";
    }
}
