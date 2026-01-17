using System;
using System.Collections.Generic;
using Ablet.API.V1.Querying;

namespace Ablet.Querying
{
    public partial class AQueryContext : API.V1.Querying.AQueryContext
    {
        public static AQueryContext Immediate => new AQueryContext(true, true);

        static AQueryContext? _current;

        readonly bool _immediate;
        readonly Queue<Action> _actions = new Queue<Action>();
        
        internal bool AllowResolve;

        public AQuery<T> Query<T>(AQueryResolver<T> resolver) => new AQueryImpl<T>(this, resolver);

        internal AQueryContext(bool immediate, bool allowImmediateResolve)
        {
            _immediate = immediate;
            AllowResolve = allowImmediateResolve;
        }
        
        public static void Enqueue(Action action)
        {
            (_current ?? Immediate).DoEnqueue(action);
        }

        void DoEnqueue(Action action)
        {
            _actions.Enqueue(action);
            if (_immediate)
            {
                AllowAndResolveAll();
            }
        }

        void AllowAndResolveAll()
        {
            AllowResolve = true;
            while (_actions.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }

        public IDisposable ResolveScope()
        {
            return new ResolveScopeDisposable(this);
        }

        internal class ResolveScopeDisposable : IDisposable
        {
            public ResolveScopeDisposable(AQueryContext context)
            {
                if (_current != null)
                {
                    throw new InvalidOperationException();
                }
                _current = context;
            }

            public void Dispose()
            {
                try
                {
                    _current?.AllowAndResolveAll();
                }
                finally
                {
                    _current = null;
                }
            }
        }
    }
}
