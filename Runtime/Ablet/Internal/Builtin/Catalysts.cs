using Ablet.API;
using Ablet.InternalAPI;
using Ablet.InternalAPI.Attributes;

namespace Ablet.Builtin
{
    static class Catalysts
    {
        [AbletCatalyst]
        internal class RealtimePreview : IAbletCatalyst
        {
            string IAbletDefinitionBase.Id => "ablet.catalyst.realtime-preview";
            string IAbletDefinitionBase.DisplayName => "Realtime Preview";
            int IAbletDefinitionBase.Priority => int.MaxValue;

            bool IAbletCatalyst.IsUserInitiatedAction => false;
            bool IAbletCatalyst.WillPersistGeneratedAssets => false;
            bool IAbletCatalyst.IsExternalSemantics => false;
        }

        [AbletCatalyst]
        internal class AutomatedBuild : IAbletCatalyst
        {
            string IAbletDefinitionBase.Id => "ablet.catalyst.automated-build";
            string IAbletDefinitionBase.DisplayName => "Automated Build";
            int IAbletDefinitionBase.Priority => int.MaxValue;

            bool IAbletCatalyst.IsUserInitiatedAction => false;
            bool IAbletCatalyst.WillPersistGeneratedAssets => false;
            bool IAbletCatalyst.IsExternalSemantics => false;
        }

        [AbletCatalyst]
        internal class ManualExport : IAbletCatalyst
        {
            string IAbletDefinitionBase.Id => "ablet.catalyst.manual-export";
            string IAbletDefinitionBase.DisplayName => "Manual Export";
            int IAbletDefinitionBase.Priority => int.MaxValue;

            bool IAbletCatalyst.IsUserInitiatedAction => true;
            bool IAbletCatalyst.WillPersistGeneratedAssets => true;
            bool IAbletCatalyst.IsExternalSemantics => false;
        }

        [AbletCatalyst]
        internal class ApplyOnPlay : IAbletCatalyst
        {
            string IAbletDefinitionBase.Id => "ablet.catalyst.apply-on-play";
            string IAbletDefinitionBase.DisplayName => "Apply On Play";
            int IAbletDefinitionBase.Priority => int.MaxValue;

            bool IAbletCatalyst.IsUserInitiatedAction => false;
            bool IAbletCatalyst.WillPersistGeneratedAssets => false;
            bool IAbletCatalyst.IsExternalSemantics => false;
        }

        [AbletCatalyst]
        internal class PartialBuild : IAbletCatalyst
        {
            string IAbletDefinitionBase.Id => "ablet.catalyst.ndmf";
            string IAbletDefinitionBase.DisplayName => "NDMF";
            int IAbletDefinitionBase.Priority => int.MaxValue;

            bool IAbletCatalyst.IsUserInitiatedAction => false;
            bool IAbletCatalyst.WillPersistGeneratedAssets => false;
            bool IAbletCatalyst.IsExternalSemantics => true;
        }
    }
}
