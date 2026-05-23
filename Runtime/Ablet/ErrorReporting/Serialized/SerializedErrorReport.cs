using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ablet.API;

namespace Ablet.ErrorReporting.Serialized
{
    [Serializable]
    public class SerializedErrorReport : IAbletSerializedBuildReportPayload.ClipboardCopyable
    {
        public List<SerializedErrorLog> log = new List<SerializedErrorLog>();

        string IAbletSerializedBuildReportPayload.ClipboardCopyable.ToCopyableString()
        {
            return log
                .Select(l => l.ToCopyableString())
                .Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s)).ToString();

        }
    }

    [Serializable]
    public class SerializedErrorLog
    {
        public ErrorKind kind;
        public SerializedObjectChain[] interests = { };
        public SerializedTextLog text = new SerializedTextLog();
        public SerializedExceptionLog exception = new SerializedExceptionLog();

        internal string ToCopyableString() =>
            kind switch
            {
                ErrorKind.Error => text.ToCopyableString(),
                ErrorKind.Warning => text.ToCopyableString(),
                ErrorKind.Information => text.ToCopyableString(),
                ErrorKind.Exception => exception.ToCopyableString(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    [Serializable]
    public class SerializedTextLog
    {
        public string message = "";
        public string stacktrace = "";

        internal string ToCopyableString() => $"{message}\n{stacktrace}";
    }

    [Serializable]
    public class SerializedExceptionLog
    {
        public string type = "";
        public string message = "";
        public string stacktrace = "";

        internal string ToCopyableString() => $"{type}\n{message}\n{stacktrace}";
    }
}
