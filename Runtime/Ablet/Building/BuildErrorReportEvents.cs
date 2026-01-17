using System;
using Ablet.API.V1.Building;

namespace Ablet.Building
{
    static class BuildErrorReportEvents
    {
        public static Func<IDisposable>? CreateScope;
        public static Action<Exception>? OnException;
        public static Action<IBuildContext>? OnExport;
    }
}
