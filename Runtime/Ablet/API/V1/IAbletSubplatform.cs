using UnityEngine;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Subplatform configures output of an Ablet build within a Platform. 
    /// </summary>
    public interface IAbletSubplatform : IAbletDefinition
    {
        string PlatformId { get; }
        int Priority { get; }
        bool IsAvailable { get; }
        bool IsPreferredSubplatform(GameObject entrypointObject);
    }
}
