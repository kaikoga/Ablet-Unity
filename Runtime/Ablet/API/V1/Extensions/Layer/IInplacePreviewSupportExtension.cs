using Ablet.API.V1.Building;

namespace Ablet.API.V1.Extensions.Layer
{
    public interface IInplacePreviewSupportExtension : IAbletExtension
    {
        AbletObservableProcedure? ToProcedure(IBuildArgument argument);
    }
}
