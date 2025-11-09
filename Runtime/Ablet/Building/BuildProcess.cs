using System.Collections.Generic;
using Ablet.Planning;

namespace Ablet.Building
{
    public class BuildProcess
    {
        readonly IEnumerable<AbletPass> _plan;
        readonly BuildContext _context;

        BuildProcess(IEnumerable<AbletPass> plan, BuildArgument argument)
        {
            _plan = plan;
            _context = new BuildContext(argument);
        }

        public static BuildProcess FromPlan(IEnumerable<AbletPass> plan, BuildArgument argument)
        {
            return new BuildProcess(plan, argument);
        }

        public static BuildProcess FromSinglePass(AbletPass pass, BuildArgument argument)
        {
            return new BuildProcess(new[] { pass }, argument);
        }

        public void Build()
        {
            foreach (var pass in _plan)
            {
                pass.Layer.Processor(_context.Argument)?.Process(_context);
            }
        }
    }
}
