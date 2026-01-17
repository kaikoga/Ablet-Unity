using Ablet.Builtin.NdmfEmu;
using Ablet.Hooks;
using Anatawa12.AvatarOptimizer.API;
using UnityEngine;

namespace Ablet.AAO.Ndmf.Editor
{
    [ComponentInformation(typeof(NdmfAbletContextHolder))]
    [ComponentInformation(typeof(AbletManualAppliedTag))]
    class EntrypointComponentInformation : ComponentInformation<Component>
    {
        protected override void CollectDependency(Component component, ComponentDependencyCollector collector)
        {
            collector.MarkEntrypoint();
        }
    }
}
