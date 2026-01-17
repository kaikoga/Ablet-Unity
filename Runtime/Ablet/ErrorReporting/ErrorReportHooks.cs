using Ablet.API.V1.Attributes;
using Ablet.Building;
using Ablet.ErrorReporting.Dependencies;

namespace Ablet.ErrorReporting
{
    static class ErrorReportHooks
    {
        [AbletInitializeOnLoadMethod]
        static void AbletInitializeOnLoad()
        {
            BuildErrorReportEvents.CreateScope += () => new ErrorReportContextScope();
            BuildErrorReportEvents.OnException += ErrorReport.LogException;
            BuildErrorReportEvents.OnExport += context => ErrorOutput.Instance.Export(context);
        }
    }
}