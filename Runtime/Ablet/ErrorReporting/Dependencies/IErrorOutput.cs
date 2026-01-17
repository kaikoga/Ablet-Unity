using System;
using Ablet.API.V1.Building;
using Ablet.ErrorReporting.Ephemeral;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Dependencies
{
    interface IErrorOutput
    {
        IDisposable CreateExternalInterestScope(Object context);
        void AddExternalObjectMapping(Object from, Object to);
        void Trace(ErrorLog log);
        void Export(IBuildContext context);
    }
}
