using System;
using System.Collections.Generic;
using Ablet.API.V1.Building;
using Ablet.ErrorReporting;
using Ablet.ErrorReporting.Dependencies;
using Ablet.ErrorReporting.Ephemeral;
using Ablet.Repositories;
using Ablet.Utils;
using nadena.dev.ndmf;
using UnityEditor;

using BuildContext = Ablet.Building.BuildContext;
using NdmfErrorReport = nadena.dev.ndmf.ErrorReport;
using Object = UnityEngine.Object;

namespace Ablet.Ndmf
{
    class MaybeNdmfErrorOutput : IErrorOutput
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            UseNdmfErrorOutput();
        }

        static void UseNdmfErrorOutput()
        {
            ErrorOutput.Instance = new MaybeNdmfErrorOutput(ErrorOutput.Instance);
        }

        readonly IErrorOutput _errorOutput;
        readonly Queue<LazyDisposable> _lazy = new Queue<LazyDisposable>();

        void MaterializeLazy()
        {
            while (_lazy.TryDequeue(out var lazy))
            {
                lazy.Materialize();
            }
        }

        MaybeNdmfErrorOutput(IErrorOutput errorOutput)
        {
            _errorOutput = errorOutput;
        }

        IDisposable IErrorOutput.CreateExternalInterestScope(Object context)
        {
            // NOTE:
            // - Ablet allows new InterestScope() adding ObjectMapping.Register()
            // - NDMF does not allow ErrorReport.WithContextObject() before ObjectRegistry.RegisterReplacedObject()  
            var lazy = new LazyDisposable(() => NdmfErrorReport.WithContextObject(context));
            _lazy.Enqueue(lazy);
            return new CompositeDisposable(
                lazy,
                _errorOutput.CreateExternalInterestScope(context)
            );
        }

        void IErrorOutput.AddExternalObjectMapping(Object from, Object to)
        {
            ObjectRegistry.RegisterReplacedObject(from, to);
            _errorOutput.AddExternalObjectMapping(from, to);
        }

        void IErrorOutput.Trace(ErrorLog log)
        {
            MaterializeLazy();
            // NOTE: Trace() is supplementary for Ablet (logging only, data source for ErrorReport UI is constructed in Export()) 
            if (BuildContext.Current != null)
            {
                // We are in an Ablet build (may be either NDMF on Ablet or Ablet on NDMF)
                switch (EditorSettingsRepository.Instance.Value.NdmfInteropMode)
                {
                    case NdmfInteropMode.None:
                        // Ablet is building and NDMF interop is not available, so it is okay to output as Ablet
                        _errorOutput.Trace(log);
                        break;
                    case NdmfInteropMode.AbletOnNdmf:
                        // Ablet building is not supported, so copy all Ablet errors to NDMF ErrorReport UI (in Ablet flavor)
                        // NDMF preferred Hybrid plugin would be detected as NdmfAbletLayer but is too rare
                        NdmfWrappedLogNow(log);
                        break;
                    case NdmfInteropMode.NdmfOnAblet:
                        // We are sure Ablet Console is primary, so this is for logging NDMF preferred Hybrid plugins to NDMF ErrorReport UI
                        NdmfRawLogNow(log);
                        break;
                }
            }
            else
            {
                // At least Ablet is not building (We can't know whether NDMF is building or not)
                // This is most likely to be a NDMF preferred Hybrid plugin (then fallback to NDMF for NDMF ErrorReport UI)
                // Less likely to be outside NDMF build (then fallback to NDMF for Debug.Log())
                NdmfRawLogNow(log);
            }
        }

        void IErrorOutput.Export(IBuildContext context)
        {
            MaterializeLazy();
            _errorOutput.Export(context);
        }

        void NdmfRawLogNow(ErrorLog log)
        {
            switch (log.Kind)
            {
                case ErrorKind.Exception:
                {
                    nadena.dev.ndmf.ErrorReport.ReportException(log.Exception);
                    break;
                }
                case ErrorKind.Error:
                {
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.Error, log.Message, log.Interests));
                    break;
                }
                case ErrorKind.Warning:
                {
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, log.Message, log.Interests));
                    break;
                }
                case ErrorKind.Information:
                {
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.Information, log.Message, log.Interests));
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        void NdmfWrappedLogNow(ErrorLog log)
        {
            var layerDisplayName = log.Layer?.DisplayName ?? "Ablet";
            switch (log.Kind)
            {
                case ErrorKind.Exception:
                {
                    var message = $"{layerDisplayName} でエラーが発生しました。";
                    nadena.dev.ndmf.ErrorReport.ReportException(new AbletLayerWrappedException(message, log.Exception!));
                    break;
                }
                case ErrorKind.Error:
                {
                    var message = $"{layerDisplayName} でエラーが発生しました:\n{log.Message}";
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.Error, message, log.Interests));
                    break;
                }
                case ErrorKind.Warning:
                {
                    var message = $"{layerDisplayName} で警告が発生しました:\n{log.Message}";
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, message, log.Interests));
                    break;
                }
                case ErrorKind.Information:
                {
                    var message = $"{layerDisplayName}:\n{log.Message}";
                    nadena.dev.ndmf.ErrorReport.ReportError(new WrappedError(ErrorSeverity.Information, message, log.Interests));
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    class AbletLayerWrappedException : Exception
    {
        public AbletLayerWrappedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
