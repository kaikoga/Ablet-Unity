using Ablet.Building;
using Ablet.ErrorReporting.Dependencies;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting
{
    public static class ObjectChain
    {
        public static void Register(Object from, Object to)
        {
            var context = BuildContext.Current;
            if (context != null)
            {
                ErrorReportContextScope.Current?.objectChainRepository.Add(context, from, to);
            }
            ErrorOutput.Instance.AddExternalObjectMapping(from, to);
        }
    }
}
