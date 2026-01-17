namespace Ablet.API
{
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
