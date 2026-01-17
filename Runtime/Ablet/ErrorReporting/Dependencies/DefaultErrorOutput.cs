using System;
using System.Linq;
using Ablet.API.V1.Building;
using Ablet.ErrorReporting.Ephemeral;
using Ablet.ErrorReporting.Serialized;
using Ablet.Repositories;
using Ablet.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Dependencies
{
    class DefaultErrorOutput : IErrorOutput
    {
        IDisposable IErrorOutput.CreateExternalInterestScope(Object context)
        {
            return EmptyDisposable.Instance;
        }

        void IErrorOutput.AddExternalObjectMapping(Object from, Object to)
        {
        }

        void IErrorOutput.Trace(ErrorLog log)
        {
            switch (log.Kind)
            {
                case ErrorKind.Exception:
                    Debug.LogException(log.Exception);
                    break;
                case ErrorKind.Error:
                    Debug.LogError(log.Message, log.Interests.FirstOrDefault());
                    break;
                case ErrorKind.Warning:
                    Debug.LogWarning(log.Message, log.Interests.FirstOrDefault());
                    break;
                case ErrorKind.Information:
                    Debug.Log(log.Message, log.Interests.FirstOrDefault());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Export(IBuildContext context)
        {
            var serializedBuildReports = ErrorReportSerializer.Export(context);
            foreach (var serializedBuildReport in serializedBuildReports)
            {
                // Debug.LogError(JsonUtility.ToJson(serializedBuildReport));
                BuildReportRepository.Instance.Add(serializedBuildReport);
            }
        }
    }
}
