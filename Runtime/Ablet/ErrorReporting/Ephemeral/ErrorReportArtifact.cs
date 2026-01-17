using System.Collections.Generic;

namespace Ablet.ErrorReporting.Ephemeral
{
    class ErrorReportArtifact
    {
        readonly List<ErrorLog> _logs = new List<ErrorLog>();
        
        public IEnumerable<ErrorLog> Logs => _logs;

        public void AddLog(ErrorLog log) => _logs.Add(log);
    }
}
