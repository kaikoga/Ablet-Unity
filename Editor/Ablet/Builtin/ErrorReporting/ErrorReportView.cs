using Ablet.ErrorReporting.Serialized;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Builtin.ErrorReporting
{
    class ErrorReportView : VisualElement
    {
        public void Draw(SerializedErrorReport errorReport, GameObject? entrypointObject)
        {
            foreach (var errorLog in errorReport.log)
            {
                var errorLogView = new ErrorLogView();
                errorLogView.Draw(errorLog, entrypointObject);
                hierarchy.Add(errorLogView);
            }
        }
    }
}
