using System.Collections.Generic;
using System.Linq;
using Ablet.Models;
using Ablet.Registries;
using Ablet.Utils;

namespace Ablet.Planning
{
    public static class AvatarBuildPlanner
    {
        public const string DefaultRootLayerId = "Ablet.Root.Avatar";

        static AbletLayer DefaultRootLayer()
        {
            LayerRegistry.Instance.TryGetById(DefaultRootLayerId, out var layer);
            return layer!;
        }

        public static IEnumerable<AbletPass> PartialPlan(IEnumerable<AbletLayer> allLeafLayers, AbletLayer phase)
        {
            return BuildPlanner.Plan(allLeafLayers, phase);
        }

        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers)
        {
            return BuildPlanner.Plan(allLeafLayers, DefaultRootLayer());
        }
    }

    public static class BuildPlanner
    {
        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers, AbletLayer rootLayer)
        {
            var layerDeps = LayerDependencySet.Build(allLeafLayers);
            var resolvedLayers = ResolveLayers(rootLayer, layerDeps);
            return CreatePlan(resolvedLayers).ToArray();
        }

        static IEnumerable<LayerDependency> ResolveLayers(AbletLayer rootLayer, LayerDependencySet layerDeps)
        {
            var layersToResolve = new DistinctQueue<(AbletLayer layer, int depth), AbletLayer>(r => r.layer);
            layersToResolve.EnqueueDistinct((rootLayer, 0));
            var depsResolved = new List<LayerDependency>();
            while (layersToResolve.TryDequeue(out var resolvingLayerDepth))
            {
                var (resolvingLayer, depth) = resolvingLayerDepth; 
                var layerDep = layerDeps.GetLayerDependency(resolvingLayer);
                layerDep.Depth = depth;
                foreach (var dependent in layerDep.Dependents)
                {
                    layersToResolve.EnqueueDistinct((dependent, depth + 1));
                }
                foreach (var dependency in layerDep.Dependencies)
                {
                    layersToResolve.EnqueueDistinct((dependency, depth + 1));
                }
                depsResolved.Add(layerDep);
            }
            return depsResolved;
        }

        static IEnumerable<AbletPass> CreatePlan(IEnumerable<LayerDependency> layerDeps)
        {
            var layersPlanned = new List<AbletPass>();
            var layerDepsToPlan = new List<LayerDependency>();
            layerDepsToPlan.AddRange(
                layerDeps
                    .OrderBy(layerDep => layerDep.Layer.LayerPriority)
                    .ThenBy(layerDep => layerDep.Layer.IdForPriority)
                    .ThenBy(layerDep => layerDep.Layer.InnerPriority));
            while (layerDepsToPlan.Count > 0)
            {
                var nextLayer = layerDepsToPlan
                    .FirstOrDefault(layer => layer.Dependencies.All(dep => layersPlanned.Any(pass => pass.Layer == dep)))
                    ?? layerDepsToPlan[0];
                layerDepsToPlan.Remove(nextLayer);
                layersPlanned.Add(nextLayer.ToPass());
            }
            return layersPlanned;
        }
    }
}
