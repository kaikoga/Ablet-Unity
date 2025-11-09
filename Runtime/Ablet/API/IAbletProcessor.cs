using Ablet.Building;

namespace Ablet.API
{
    /// <summary>
    /// An Ablet Processor is the actual procedural operation triggered from an Ablet Layer.
    /// </summary>
    public interface IAbletProcessor
    {
        void Process(BuildContext buildContext);
    }
}
