using Ablet.API.V1.Building;
using Ablet.Models;

namespace Ablet.Building
{
    class AbletPlatformHandle : IAbletPlatformHandle
    {
        readonly IAbletIdModelBase _platform;
        public AbletPlatformHandle(AbletPlatform platform) => _platform = platform;
        public AbletPlatformHandle(AbletSubplatform subplatform) => _platform = subplatform;

        public string Id => _platform.Id;
        public string DisplayName => _platform.DisplayName;
    }
}
