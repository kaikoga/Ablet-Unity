using Ablet.API;

namespace Ablet.InternalAPI
{
    interface IAbletCatalyst : IAbletDefinitionBase
    {
        bool IsUserInitiatedAction { get; }
        bool WillPersistGeneratedAssets { get; }
        bool IsExternalSemantics { get; }
    }
}
