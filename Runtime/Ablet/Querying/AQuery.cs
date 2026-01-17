using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1.Querying;

namespace Ablet.Querying
{
    public static partial class AQuery
    {
        static AQuery<TOut> Create<TIn, TOut>(this AQuery<TIn> query, AQueryResolver<TOut> resolver) => query.Context.Query(resolver);

        public static IEnumerable<TOut> ResolveNow<T, TOut>(this AQuery<T> query, Func<T, IEnumerable<TOut>> selector) => query.ResolveNow().SelectMany(selector);
    }

    class AQueryImpl<T> : AQuery<T>
    {
        readonly AQueryContext _context;
        readonly AQueryResolver<T> _resolver;

        API.V1.Querying.AQueryContext AQuery<T>.Context => _context;

        internal AQueryImpl(AQueryContext context, AQueryResolver<T> resolver)
        {
            _context = context;
            _resolver = resolver;
        }

        public IEnumerable<T> ResolveNow() => _context.AllowResolve ? _resolver.ResolveNow() : throw new InvalidOperationException();

        public void Observe(Action<T> filter)
        {
            AQueryContext.Enqueue(() =>
            {
                foreach (var item in ResolveNow())
                {
                    filter(item);
                }
            });
        }
    }
}
