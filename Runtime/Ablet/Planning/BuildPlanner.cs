using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.API.Internal;
using Ablet.Builtin;
using Ablet.Builtin.Utils;
using Ablet.Repositories;
using Ablet.Utils;

namespace Ablet.Planning
{
    public static class BuildPlanner
    {
        public static IEnumerable<AbletPass> PartialPlan(IEnumerable<IAbletLayer> allLeafLayers, IAbletLayer phase)
        {
            using var plan = Plan(allLeafLayers).GetEnumerator();

            while (plan.MoveNext())
            {
                var pass = plan.Current;
                if (pass.Layer.Id != phase.Id) continue;
                yield return pass;
                break;
            }
            while (plan.MoveNext())
            {
                var pass = plan.Current;
                if (pass.Layer is IAbletPhase) break;
                yield return pass;
            }
        }

        public static IEnumerable<AbletPass> Plan(IEnumerable<IAbletLayer> allLeafLayers)
        {
            var rootLayer = LayerRepository.Instance.Get<PhaseContainer>();

            var layerDeps = LayerDependencySet.Build(allLeafLayers);
            var resolvedLayers = ResolveLayers(rootLayer, layerDeps);
            return CreatePlan(rootLayer, resolvedLayers).ToArray();
        }

        static IEnumerable<LayerDependency> ResolveLayers(IAbletLayer rootLayer, LayerDependencySet layerDeps)
        {
            var layersToResolve = new DistinctQueue<IAbletLayer>();
            layersToResolve.EnqueueDistinct(rootLayer);
            var depsResolved = new List<LayerDependency>();
            while (layersToResolve.TryDequeue(out var resolvingLayer))
            {
                var layerDep = layerDeps.GetLayerDependency(resolvingLayer);
                foreach (var dependent in layerDep.Dependents)
                {
                    layersToResolve.EnqueueDistinct(dependent);
                }
                depsResolved.Add(layerDep);
            }
            return depsResolved;
        }
        
        static IEnumerable<AbletPass> CreatePlan(IAbletLayer rootLayer, IEnumerable<LayerDependency> layerDeps)
        {
            var layersPlanned = new List<AbletPass>
            {
                new AbletPass(rootLayer)
            };
            var layerDepsToPlan = new List<LayerDependency>();
            layerDepsToPlan.AddRange(
                layerDeps.Where(layerDep => layerDep.Layer != rootLayer)
                    .OrderBy(layerDep => LayerPriority(layerDep.Layer))
                    .ThenBy(layerDep => layerDep.Layer.Id));
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

        static long LayerPriority(IAbletLayer layer)
        {
            return layer switch
            {
                BeforeLayer<PhaseContainer> => int.MaxValue - 3L,
                AfterLayer<PhaseContainer> => int.MaxValue + 3L,
                IAbletPhase => int.MaxValue + 2L,
                // maybe handle inner before / after layers first?
                IBeforeLayer => int.MinValue - 1L,
                IAfterLayer => int.MaxValue + 1L,
                _ => layer.Priority
            };
        }
    }
}
