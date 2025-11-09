using System;
using System.Collections.Generic;
using Ablet.API;
using Ablet.Repositories;
using Ablet.Utils;

namespace Ablet.Planning
{
    public class LayerDependency
    {
        public readonly IAbletLayer Layer;

        readonly HashSet<IAbletLayer> _dependencies = new HashSet<IAbletLayer>();
        public IEnumerable<IAbletLayer> Dependencies => _dependencies;

        readonly HashSet<IAbletLayer> _dependents = new HashSet<IAbletLayer>();
        public IEnumerable<IAbletLayer> Dependents => _dependents;

        public void TryAddDependency(IAbletLayer layer)
        {
            if (layer != null) _dependencies.Add(layer);
        }

        public void TryAddDependent(IAbletLayer layer)
        {
            if (layer != null) _dependents.Add(layer);
        }

        public LayerDependency(IAbletLayer layer)
        {
            Layer = layer;
        }

        public AbletPass ToPass()
        {
            return new AbletPass(Layer);
        }
    }

    class DependencyConfiguratorImpl : IDependencyConfigurator, IDisposable
    {
        readonly LayerDependencySet _repository;
        readonly LayerDependency _dependency;
        public readonly List<IAbletLayer> NonleafLayerReferences = new List<IAbletLayer>();

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
                throw new ObjectDisposedException("Disposed");
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
                throw new ObjectDisposedException("Disposed");
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
                throw new ObjectDisposedException("Disposed");
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
                throw new ObjectDisposedException("Disposed");
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

    public class LayerDependencySet
    {
        readonly Dictionary<Type, LayerDependency> _layerDependencies = new Dictionary<Type, LayerDependency>();
        
        public IEnumerable<LayerDependency> All() => _layerDependencies.Values;
        public LayerDependency GetLayerDependency<T>() where T : IAbletLayer => GetLayerDependency(LayerRepository.Instance.Get<T>());

        public bool TryGetLayerDependency(string id, out LayerDependency layerDep)
        {
            if (LayerRepository.Instance.TryGetById(id, out var layer))
            {
                layerDep = GetLayerDependency(layer);
                return true;
            }
            layerDep = null;
            return false;
        }

        public LayerDependency GetLayerDependency(IAbletLayer layer)
        {
            var type = layer.GetType();
            if (_layerDependencies.TryGetValue(type, out var existing))
            {
                return existing;
            }
            var value = new LayerDependency(layer);
            _layerDependencies.Add(type, value);
            return value;
        }

        public static LayerDependencySet Build(IEnumerable<IAbletLayer> allLeafLayers)
        {
            var layersToConfigure = new DistinctQueue<IAbletLayer>();
            foreach (var leafLayer in allLeafLayers) layersToConfigure.EnqueueDistinct(leafLayer);
            var repo = new LayerDependencySet();
            while (layersToConfigure.TryDequeue(out var layer))
            {
                using var config = new DependencyConfiguratorImpl(repo, repo.GetLayerDependency(layer));
                layer.Configure(config);
                foreach (var nonleafLayerRef in config.NonleafLayerReferences)
                {
                    layersToConfigure.EnqueueDistinct(nonleafLayerRef);
                }
            }
            return repo;
        }
    }
}
