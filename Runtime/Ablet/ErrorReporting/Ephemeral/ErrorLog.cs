using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Building;
using Ablet.ErrorReporting.Serialized;
using Ablet.Models;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Ephemeral
{
    class ErrorLog
    {
        public readonly AbletLayer? Layer;
        public readonly ErrorKind Kind;
        public readonly string Message;
        public readonly Object[] Interests;
        public readonly SerializedObjectReference[] SerializedInterests;
        public readonly Exception? Exception;
        public readonly string? ExtraStackTrace;
        
        ErrorLog(AbletLayer? layer, ErrorKind kind, Exception exception, string extraStackTrace, IEnumerable<Object> interests)
        {
            Layer = layer;
            Kind = kind;
            Message = "";
            Exception = exception;
            ExtraStackTrace = extraStackTrace;
            Interests = interests.ToArray();
            SerializedInterests = BuildContext.Current != null
                ? ErrorReportSerializer.Export(Interests, BuildContext.Current.CurrentRootObject).ToArray()
                : Array.Empty<SerializedObjectReference>();
        }

        ErrorLog(AbletLayer? layer, ErrorKind kind, string message, IEnumerable<Object> contexts)
        {
            Layer = layer;
            Kind = kind;
            Message = message;
            Interests = contexts.ToArray();
            SerializedInterests = BuildContext.Current != null
                ? ErrorReportSerializer.Export(Interests, BuildContext.Current.CurrentRootObject).ToArray()
                : Array.Empty<SerializedObjectReference>();
        }

        internal static ErrorLog AsException(AbletLayer? layer, Exception exception, string extraStackTrace, IEnumerable<Object> interests)
        {
            return new ErrorLog(layer, ErrorKind.Exception, exception, extraStackTrace, interests);
        }

        internal static ErrorLog AsError(AbletLayer? layer, string message, IEnumerable<Object> interests)
        {
            return new ErrorLog(layer, ErrorKind.Error, message, interests);
        }

        internal static ErrorLog AsWarning(AbletLayer? layer, string message, IEnumerable<Object> interests)
        {
            return new ErrorLog(layer, ErrorKind.Warning, message, interests);
        }

        internal static ErrorLog AsInformation(AbletLayer? layer, string message, IEnumerable<Object> interests)
        {
            return new ErrorLog(layer, ErrorKind.Information, message, interests);
        }
    }
}
