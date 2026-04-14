using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Ablet.API.V1.Building
{
    public interface IBuildArgument
    {
        GameObject EntrypointObject { get; }
        IAbletPlatformHandle? EntrypointPlatform { get; }
        IAbletPlatformHandle TargetPlatform { get; }

        AssetGenerationMode WillCloneSceneObject { get; }
        AssetGenerationMode WillPersistGeneratedAssets { get; }
        bool IsPartial { get; }
        BuildInitiationSourceMode BuildInitiationSourceMode { get; }

        bool TryGetInput<T>([MaybeNullWhen(false)] out T value) where T : class;
    }
}
