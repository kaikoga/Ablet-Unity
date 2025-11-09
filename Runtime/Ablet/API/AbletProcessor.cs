using System;
using Ablet.Building;

namespace Ablet.API
{
    public class AbletProcessor : IAbletProcessor
    {
        readonly Action<BuildContext> _processor;

        public AbletProcessor(Action<BuildContext> processor) => _processor = processor;

        public void Process(BuildContext buildContext) => _processor?.Invoke(buildContext);
    }
}
