using System;
using Ablet.API.V1.Building;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Procedure is the actual procedural operation triggered from an Ablet Layer.
    /// AbletProcedure should be either AbletBuildProcedure or AbletObservableProcedure.
    /// </summary>
    public abstract class AbletProcedure
    {
        // Prevent custom subclass
        internal AbletProcedure() { }
    }

    /// <summary>
    /// An Ablet Procedure that can be applied oneshot and synchronously.
    /// </summary>
    public abstract class AbletBuildProcedure : AbletProcedure
    {
        public abstract void Process(IBuildContext context);

        public static AbletBuildProcedure Create(Action<IBuildContext> procedure) => new InlineBuildProcedure(procedure);

        class InlineBuildProcedure : AbletBuildProcedure
        {
            readonly Action<IBuildContext> _procedure;

            public InlineBuildProcedure(Action<IBuildContext> procedure) => _procedure = procedure;

            public override void Process(IBuildContext context) => _procedure.Invoke(context);
        }
    }

    /// <summary>
    /// An Ablet Procedure that is rerunnable and fine-grained.
    /// </summary>
    public abstract class AbletObservableProcedure : AbletProcedure
    {
        public abstract void Observe(IObserveContext context);
        
        public static AbletObservableProcedure Create(Action<IObserveContext> procedure) => new InlineObservableProcedure(procedure);

        class InlineObservableProcedure : AbletObservableProcedure
        {
            readonly Action<IObserveContext> _procedure;

            public InlineObservableProcedure(Action<IObserveContext> procedure) => _procedure = procedure;

            public override void Observe(IObserveContext context) => _procedure.Invoke(context);
        }
    }
}
