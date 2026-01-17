using System.Linq;
using System.Text;
using Ablet.Planning;
using Ablet.Registries;
using UnityEditor;
using UnityEngine;

namespace Ablet.Debugging
{
    static class AbletDebugStaticMenuItems
    {
        [MenuItem("Tools/Ablet/Debug/Show Build Plan", false, 100)]
        static void DebugShowBuildPlan()
        {
            var plan = AvatarBuildPlanner.Plan(LayerRegistry.Instance.All());
            Debug.LogError(plan.Aggregate(new StringBuilder(), (sb, pass) =>
            {
                sb.AppendLine($"{string.Join("", Enumerable.Repeat("  ", pass.Depth))} {pass.Layer.Id}: {pass.Layer.DisplayName}");
                return sb;
            }));
        }
        
        [MenuItem("Tools/Ablet/Debug/Show Layer Dependency", false, 100)]
        static void DebugShowLayerDependency()
        {
            var layerDeps = LayerDependencySet.Build(LayerRegistry.Instance.All());
            Debug.LogError(layerDeps.All().Aggregate(new StringBuilder(), (sb, layerDep) =>
            {
                sb.AppendLine(layerDep.Layer.Id);
                foreach (var dependency in layerDep.Dependencies)
                {
                    sb.AppendLine($"< {dependency.Id}: {dependency.DisplayName}");
                }
                foreach (var dependent in layerDep.Dependents)
                {
                    sb.AppendLine($"> {dependent.Id}: {dependent.DisplayName}");
                }
                return sb;
            }));
        }
    }
}
