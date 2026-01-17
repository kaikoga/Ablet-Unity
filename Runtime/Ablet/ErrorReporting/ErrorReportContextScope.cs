using System;
using Ablet.ErrorReporting.Ephemeral;

namespace Ablet.ErrorReporting
{
    public class ErrorReportContextScope : IDisposable
    {
        internal static ErrorReportContextScope? Current;

        internal readonly InterestRepository InterestRepository = new InterestRepository();
        internal readonly ObjectChainRepository objectChainRepository = new ObjectChainRepository();

        public ErrorReportContextScope()
        {
            if (Current != null)
            {
                throw new InvalidOperationException("CreateErrorContext already created");
            }
            Current = this;
        }

        public void Dispose()
        {
            InterestRepository.Dispose();
            Current = null;
        }
    }
}
