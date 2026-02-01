using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.API.V1;
using Ablet.Models;
using Ablet.Registries;
using Ablet.Utils;

namespace Ablet.Planning
{
    class LayerDependency
    {
        public readonly AbletLayer Layer;
        public int Depth;
        readonly bool _isContainerPass;

        readonly HashSet<AbletLayer> _dependencies = new HashSet<AbletLayer>();
        public IEnumerable<AbletLayer> Dependencies => _dependencies;

        readonly HashSet<AbletLayer> _dependents = new HashSet<AbletLayer>();
        public IEnumerable<AbletLayer> Dependents => _dependents;

        public void TryAddDependency(AbletLayer? layer)
        {
            if (layer != null) _dependencies.Add(layer);
        }

        public void TryAddDependent(AbletLayer? layer)
        {
            if (layer != null) _dependents.Add(layer);
        }

        public LayerDependency(AbletLayer layer, bool isContainerPass)
        {
            Layer = layer;
            _isContainerPass = isContainerPass;
        }

        public AbletPass ToPass()
        {
            return new AbletPass(Layer, _isContainerPass, Depth);
        }
    }

    class DependencyConfiguratorImpl : IDependencyConfigurator, IDisposable
    {
        readonly LayerDependencySet _repository;
        readonly LayerDependency _dependency;
        public readonly List<AbletLayer> NonleafLayerReferences = new List<AbletLayer>();

        bool _disposed;

        public DependencyConfiguratorImpl(LayerDependencySet repository, LayerDependency dependency)
        {
            _repository = repository;
            _dependency = dependency;
        }

        public IDependencyConfigurator AddDependency(string id)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(IDependencyConfigurator));
            }
            if (_repository.TryGetLayerDependency(id, out var layerDep))
            {
                _dependency.TryAddDependency(layerDep.Layer);
                layerDep.TryAddDependent(_dependency.Layer);
                NonleafLayerReferences.Add(layerDep.Layer);
            }
            return this;
        }

        public IDependencyConfigurator AddDependency<T>()
            where T : IAbletLayer
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(IDependencyConfigurator));
            }
            var layerDep = _repository.GetLayerDependency<T>();
            _dependency.TryAddDependency(layerDep.Layer);
            layerDep.TryAddDependent(_dependency.Layer);
            NonleafLayerReferences.Add(layerDep.Layer);
            return this;
        }

        public IDependencyConfigurator AddReverseDependency(string id)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(IDependencyConfigurator));
            }
            if (_repository.TryGetLayerDependency(id, out var layerDep))
            {
                layerDep.TryAddDependency(_dependency.Layer);
                _dependency.TryAddDependent(layerDep.Layer);
                NonleafLayerReferences.Add(layerDep.Layer);
            }
            return this;
        }

        public IDependencyConfigurator AddReverseDependency<T>()
            where T : IAbletLayer
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(IDependencyConfigurator));
            }
            var layerDep = _repository.GetLayerDependency<T>();
            layerDep.TryAddDependency(_dependency.Layer);
            _dependency.TryAddDependent(layerDep.Layer);
            NonleafLayerReferences.Add(layerDep.Layer);
            return this;
        }

        void IDisposable.Dispose()
        {
            _disposed = true;
        }
    }

    class LayerDependencySet
    {
        readonly Dictionary<Type, LayerDependency> _layerDependencies = new Dictionary<Type, LayerDependency>();
        readonly Dictionary<Type, LayerDependency> _concreteLayers = new Dictionary<Type, LayerDependency>();
        
        public IEnumerable<LayerDependency> All() => _layerDependencies.Values.Concat(_concreteLayers.Values);
        public LayerDependency GetLayerDependency<T>() where T : IAbletLayer => GetLayerDependency(LayerRegistry.Instance.Get<T>());

        public bool TryGetLayerDependency(string id, [MaybeNullWhen(false)] out LayerDependency layerDep)
        {
            if (LayerRegistry.Instance.TryGetById(id, out var layer))
            {
                layerDep = GetLayerDependency(layer);
                return true;
            }
            layerDep = null;
            return false;
        }

        public LayerDependency GetLayerDependency(AbletLayer layer)
        {
            var type = layer.DefType;
            if (_layerDependencies.TryGetValue(type, out var existing))
            {
                return existing;
            }
            var value = new LayerDependency(layer, true);
            _layerDependencies.Add(type, value);
            return value;
        }

        public LayerDependency GetConcreteLayer(AbletLayer layer)
        {
            var type = layer.DefType;
            if (_concreteLayers.TryGetValue(type, out var existing))
            {
                return existing;
            }
            var value = new LayerDependency(layer, false);
            value.TryAddDependency(layer);
            _concreteLayers.Add(type, value);
            return value;
        }

        public static LayerDependencySet Build(IEnumerable<AbletLayer> allLeafLayers)
        {
            var layersToConfigure = new DistinctQueue<AbletLayer>();
            foreach (var leafLayer in allLeafLayers) layersToConfigure.EnqueueDistinct(leafLayer);
            var layerDeps = new LayerDependencySet();
            while (layersToConfigure.TryDequeue(out var layer))
            {
                using var config = new DependencyConfiguratorImpl(layerDeps, layerDeps.GetLayerDependency(layer));
                layer.Configure(config);
                foreach (var nonleafLayerRef in config.NonleafLayerReferences)
                {
                    layersToConfigure.EnqueueDistinct(nonleafLayerRef);
                }
            }
            return layerDeps;
        }
    }
}
