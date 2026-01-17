using System;
using Ablet.ErrorReporting.Dependencies;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting
{
    public class InterestScope : IDisposable
    {
        readonly Object _interest;
        readonly IDisposable _externalScope;

        public InterestScope(Object interest)
        {
            _interest = interest;
            ErrorReportContextScope.Current?.InterestRepository.Add(interest);
            _externalScope = ErrorOutput.Instance.CreateExternalInterestScope(interest);
        }

        public void Dispose()
        {
            _externalScope.Dispose();
            ErrorReportContextScope.Current?.InterestRepository.Remove(_interest);
        }
    }
}
