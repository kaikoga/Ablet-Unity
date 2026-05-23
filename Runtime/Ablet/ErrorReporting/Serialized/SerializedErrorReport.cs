using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ablet.API;
using UnityEngine;

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

        internal static string PrettyPrintStackTrace(string stackTrace)
        {
            const string projectRoot = "/%PROJECT_ROOT%";
            var projectPath = Path.GetDirectoryName(Application.dataPath) ?? projectRoot;
            return stackTrace.Replace(projectPath, projectRoot);
        }
    }

    [Serializable]
    public class SerializedTextLog
    {
        public string message = "";
        public string stacktrace = "";
        
        public string PrettyStackTrace => SerializedErrorLog.PrettyPrintStackTrace(stacktrace);

        internal string ToCopyableString() => $"{message}\n{PrettyStackTrace}";
    }

    [Serializable]
    public class SerializedExceptionLog
    {
        public string type = "";
        public string message = "";
        public string stacktrace = "";

        public string PrettyStackTrace => SerializedErrorLog.PrettyPrintStackTrace(stacktrace);

        internal string ToCopyableString() => $"{type}\n{message}\n{PrettyStackTrace}";
    }
}
