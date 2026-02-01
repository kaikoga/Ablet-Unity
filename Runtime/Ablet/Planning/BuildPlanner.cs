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

        public static IEnumerable<AbletPass> PartialPlan(IEnumerable<AbletLayer> allLeafLayers, AbletLayer phase, bool withContainerPass = false)
        {
            return BuildPlanner.Plan(allLeafLayers, phase, withContainerPass);
        }

        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers, bool withContainerPass = false)
        {
            return BuildPlanner.Plan(allLeafLayers, DefaultRootLayer(), withContainerPass);
        }
    }

    public static class BuildPlanner
    {
        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers, AbletLayer rootLayer, bool withContainerPass = false)
        {
            var layerDeps = LayerDependencySet.Build(allLeafLayers);
            var resolvedLayers = ResolveLayers(rootLayer, layerDeps);
            return CreatePlan(resolvedLayers)
                .Where(pass => withContainerPass || !pass.IsContainerPass)
                .ToArray();
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
                if (resolvingLayer != rootLayer)
                {
                    // FIXME: workaround for PartialPlan
                    // building the entire dependency graph and locating the partial root layer would be better  
                    // (because cross-phase dependencies may mess up partial dependency graph)
                    foreach (var dependency in layerDep.Dependencies)
                    {
                        layersToResolve.EnqueueDistinct((dependency, depth + 1));
                    }
                }
                depsResolved.Add(layerDep);
                if (layerDep.Layer.IsConcreteLayer)
                {
                    var concreteLayer = layerDeps.GetConcreteLayer(layerDep.Layer);
                    concreteLayer.Depth = depth + 1;
                    depsResolved.Add(concreteLayer);
                }
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
                    .ThenBy(layerDep => layerDep.Layer.InnerPriority)
                    .ThenBy(layerDep => layerDep.Layer.IdForPriority));
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
