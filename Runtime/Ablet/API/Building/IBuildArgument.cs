using Ablet.API;
using UnityEngine;

namespace Ablet.Building
{
    public interface IBuildArgument
    {
        public IAbletPlatform Platform { get; }
        public GameObject EntrypointObject { get; }

        public bool IsUserInitiatedAction { get; }
        public bool WillPersistGeneratedAssets { get; }
        public bool IsExternalSemantics { get; }
    }
}
