using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Models;
using Ablet.Registries;
using Ablet.Utils;
using UnityEngine;

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
            return BuildPlanner.Plan(allLeafLayers, DefaultRootLayer(), phase, withContainerPass);
        }

        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers, bool withContainerPass = false)
        {
            return BuildPlanner.Plan(allLeafLayers, DefaultRootLayer(), DefaultRootLayer(), withContainerPass);
        }
    }

    public static class BuildPlanner
    {
        public static IEnumerable<AbletPass> Plan(IEnumerable<AbletLayer> allLeafLayers, AbletLayer rootLayer, AbletLayer phase, bool withContainerPass = false)
        {
            var layerDeps = LayerDependencySet.Build(allLeafLayers);
            var resolvedLayers = ResolveLayers(rootLayer, layerDeps);
            var tree = BuildTree(resolvedLayers);

            if (!(tree.Lookup(phase) is { } subTree))
            {
                throw new ArgumentException($"<{rootLayer.Id}> does not contain <{phase.Id}>");
            }

            return CreatePlan(subTree)
                .Where(pass => withContainerPass || !pass.IsContainerPass)
                .ToArray();
        }

        static IEnumerable<LayerDependency> ResolveLayers(AbletLayer rootLayer, LayerDependencySet layerDeps)
        {
            var layersToResolve = new DistinctQueue<AbletLayer>();
            layersToResolve.EnqueueDistinct(rootLayer);
            var depsResolved = new List<LayerDependency>();
            while (layersToResolve.TryDequeue(out var resolvingLayer))
            {
                var layerDep = layerDeps.GetLayerDependency(resolvingLayer);
                foreach (var dependent in layerDep.Dependents)
                {
                    layersToResolve.EnqueueDistinct(dependent);
                }
                if (resolvingLayer != rootLayer)
                {
                    foreach (var dependency in layerDep.Dependencies)
                    {
                        layersToResolve.EnqueueDistinct(dependency);
                    }
                }
                depsResolved.Add(layerDep);
                if (layerDep.Layer.IsConcreteLayer)
                {
                    var concreteLayer = layerDeps.GetConcreteLayer(layerDep.Layer);
                    depsResolved.Add(concreteLayer);
                }
            }
            return depsResolved;
        }

        static AbletPassTree BuildTree(IEnumerable<LayerDependency> layerDeps)
        {
            var layerDepsToPlant = new List<LayerDependency>();
            var treesPlanted = new List<AbletPassTree>();
            layerDepsToPlant.AddRange(
                layerDeps
                    .OrderBy(layerDep => layerDep.Layer.LayerPriority)
                    .ThenBy(layerDep => layerDep.Layer.InnerPriority)
                    .ThenBy(layerDep => layerDep.Layer.IdForPriority));
            while (layerDepsToPlant.Count > 0)
            {
                var layerDepToPlant = layerDepsToPlant
                    .FirstOrDefault(layerDep => layerDep.Dependencies.All(layer => treesPlanted.Any(node => node.Layer == layer)));
                if (layerDepToPlant == null)
                {
                    layerDepToPlant = layerDepsToPlant[0];
                    Debug.LogWarning($"Layer order could not be determined, force choosing {layerDepToPlant.Layer.DisplayName} ({layerDepToPlant.Layer.Id})");
                }

                var leaf = new AbletPassTree(layerDepToPlant.Layer, layerDepToPlant.IsContainerPass);
                for (var i = treesPlanted.Count - 1; i >= 0; i--)
                {
                    var node = treesPlanted[i];
                    if (node.IsContainerPass
                        && layerDepToPlant.Dependencies.Any(dep => dep == node.Layer))
                    {
                        node.Children.Add(leaf);
                        break;
                    }
                }
                treesPlanted.Add(leaf);
                layerDepsToPlant.Remove(layerDepToPlant);
            }
            return treesPlanted[0];
        }

        static IEnumerable<AbletPass> CreatePlan(AbletPassTree tree)
        {
            var layersPlanned = new List<AbletPass>();
            void PlanTree(AbletPassTree node, int depth)
            {
                layersPlanned.Add(node.ToPass(depth));
                foreach (var child in node.Children)
                {
                    PlanTree(child, depth + 1);
                }
            }
            PlanTree(tree, 0);
            return layersPlanned;
        }
    }
}
