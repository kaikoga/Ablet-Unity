using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.API.V1.Querying;
using Ablet.Planning;
using Ablet.Querying;
using Ablet.Repositories;
using UnityEngine;
using AQueryContext = Ablet.Querying.AQueryContext;

namespace Ablet.Building
{
    public class BuildContext : IBuildContext, IObserveContext, IDisposable
    {
        internal static IBuildContext? Current;

        public static bool TryGetCurrentBuildArgument([MaybeNullWhen(false)] out IBuildArgument argument)
        {
            argument = Current?.Argument;
            return argument != null;
        }

        public BuildArgument Argument { get; }
        IBuildArgument IProcessContext.Argument => Argument;

        readonly DatastoreRepository _artifacts = new DatastoreRepository();

        GameObject _rootObject;

        public readonly AQueryContext AQueryContext;

        public AQuery<GameObject> RootObject => AQueryContext.Lazy(() => _rootObject);
        public AQuery<Transform> RootTransform => RootObject.GetComponents<Transform>();
        public GameObject CurrentRootObject => _rootObject;
        public Transform CurrentRootTransform => _rootObject.transform;

        public void SetCurrentRootObject(GameObject gameObject)
        {
            _rootObject = Argument.ObjectRetainMode switch
            {
                ObjectRetainMode.None => gameObject,
                ObjectRetainMode.RetainObjectId => throw new InvalidOperationException("Cannot replace current object"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public BuildContext(BuildArgument argument, GameObject rootObject)
        {
            Current = this;
            Argument = argument;
            _rootObject = rootObject;
            AQueryContext = new AQueryContext(false, !argument.IsObservable);
        }

        public void AddArtifact<T>(T value) where T : class => _artifacts.Add(value);

        public bool TryGetArtifact<T>([MaybeNullWhen(false)] out T value) where T : class => _artifacts.TryGet(out value);
        public T GetOrCreateArtifact<T>() where T : class, new() => _artifacts.GetOrCreate<T>();

        public void Dispose() => Current = null;

        public class PassScope : IDisposable
        {
            internal static AbletPass? CurrentPass;

            public PassScope(AbletPass pass)
            {
                CurrentPass = pass;
            }

            public void Dispose()
            {
                CurrentPass = null;
            }
        }
    }
}
