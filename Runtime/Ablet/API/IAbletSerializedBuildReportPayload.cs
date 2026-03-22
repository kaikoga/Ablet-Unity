using System.Diagnostics.CodeAnalysis;

namespace Ablet.API
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IAbletSerializedBuildReportPayload
    {
        public interface WithDiscriminator : IAbletSerializedBuildReportPayload
        {
            string Discriminator { get; }
        }

        public interface WithPriority : IAbletSerializedBuildReportPayload
        {
            int Priority { get; }
        }
    }
}
