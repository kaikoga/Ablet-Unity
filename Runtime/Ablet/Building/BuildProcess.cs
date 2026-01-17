using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ablet.API.V1;
using Ablet.Planning;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Ablet.Building
{
    public class BuildProcess
    {
        readonly IEnumerable<AbletPass> _plan;
        readonly BuildArgument _argument;

        BuildProcess(IEnumerable<AbletPass> plan, BuildArgument argument)
        {
            _plan = plan;
            _argument = argument;
        }

        public static BuildProcess FromPlan(IEnumerable<AbletPass> plan, BuildArgument argument)
        {
            return new BuildProcess(plan, argument);
        }

        public static BuildProcess FromSinglePass(AbletPass pass, BuildArgument argument)
        {
            return new BuildProcess(new[] { pass }, argument);
        }

        public GameObject Build() => Build(_argument.EntrypointObject);

        public GameObject Build(GameObject rootObject)
        {
            using (BuildErrorReportEvents.CreateScope?.Invoke())
            using (var context = new BuildContext(_argument, rootObject))
            {
                var isObservable = _argument.IsObservable;
                foreach (var pass in _plan)
                {
                    ExecutePass(context, pass, isObservable);
                }
                BuildErrorReportEvents.OnExport?.Invoke(context);
                return context.CurrentRootObject;
            }
        }

        void ExecutePass(BuildContext context, AbletPass pass, bool isObservable)
        {
            using (new BuildContext.PassScope(pass))
            {
                var stopwatch = new Stopwatch();
                var processedSomething = false;
                stopwatch.Start();
                try
                {
                    using (context.AQueryContext.ResolveScope())
                    {
                        switch (pass.Layer.ToProcedure(context.Argument))
                        {
                            case AbletObservableProcedure observableProcedure:
                                observableProcedure.Observe(context);
                                processedSomething = true;
                                break;
                            case AbletBuildProcedure buildProcedure:
                                if (!isObservable)
                                {
                                    buildProcedure.Process(context);
                                    processedSomething = true;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    BuildErrorReportEvents.OnException?.Invoke(ex);
                }
                if (processedSomething)
                {
                    Debug.Log($"[Ablet] <{pass.Layer.DisplayName}> layer processed in {stopwatch.ElapsedMilliseconds}ms");
                }
            }
        }
    }
}
