using Ablet.API.V1.Querying;
using UnityEngine;

namespace Ablet.API.V1.Building
{
    public interface IProcessContext
    {
        IBuildArgument Argument { get; }

        void AddArtifact<T>(T value) where T : class;
        bool TryGetArtifact<T>(out T? value) where T : class;
        T GetOrCreateArtifact<T>() where T : class, new();
    }

    public interface IObserveContext : IProcessContext
    {
        AQuery<GameObject> RootObject { get; }
        AQuery<Transform> RootTransform { get; }
    }

    public interface IBuildContext : IProcessContext
    {
        AQuery<GameObject> RootObject { get; }
        AQuery<Transform> RootTransform { get; }
        GameObject CurrentRootObject { get; }
        Transform CurrentRootTransform { get; }
        void SetCurrentRootObject(GameObject gameObject);
    }
}
