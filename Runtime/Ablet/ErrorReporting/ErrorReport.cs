using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Building;
using Ablet.ErrorReporting.Dependencies;
using Ablet.ErrorReporting.Ephemeral;
using Ablet.Models;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting
{
    public static class ErrorReport
    {
        public static void LogException(Exception ex)
        {
            var extraStackTrace = string.Join("\n", Environment.StackTrace.Split("\n").Skip(1));
            var log = ErrorLog.AsException(CurrentLayer(), ex, extraStackTrace, CurrentInterests());
            LogNow(log);
        }

        public static void LogError(string message)
        {
            var extraStackTrace = string.Join("\n", Environment.StackTrace.Split("\n").Skip(1));
            var log = ErrorLog.AsError(CurrentLayer(), message, extraStackTrace, CurrentInterests());
            LogNow(log);
        }

        public static void LogWarning(string message)
        {
            var extraStackTrace = string.Join("\n", Environment.StackTrace.Split("\n").Skip(1));
            var log = ErrorLog.AsWarning(CurrentLayer(), message, extraStackTrace, CurrentInterests());
            LogNow(log);
        }

        public static void LogInformation(string message)
        {
            var extraStackTrace = string.Join("\n", Environment.StackTrace.Split("\n").Skip(1));
            var log = ErrorLog.AsInformation(CurrentLayer(), message, extraStackTrace, CurrentInterests());
            LogNow(log);
        }

        static AbletLayer? CurrentLayer() => BuildContext.PassScope.CurrentPass?.Layer;

        static IEnumerable<Object> CurrentInterests() => ErrorReportContextScope.Current?.InterestRepository.Objects ?? Enumerable.Empty<Object>();

        static void LogNow(ErrorLog log)
        {
            ErrorOutput.Instance.Trace(log);
            if (BuildContext.Current is { } context)
            {
                context.GetOrCreateArtifact<ErrorReportArtifact>().AddLog(log);
            }
        }
    }
}