using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1.Querying;

namespace Ablet.Querying.Resolvers
{
    class WhereQuery<T> : AQueryDecoratorResolverBase<T, T>
    {
        readonly Func<T, bool> _filter;

        public WhereQuery(AQuery<T> query, Func<T, bool> filter) : base(query)
        {
            _filter = filter;
        }

        protected override IEnumerable<T> ResolveImpl()
        {
            return Query.ResolveNow().Where(_filter);
        }
    }

    class SelectQuery<TIn, TOut> : AQueryDecoratorResolverBase<TIn, TOut>
    {
        readonly Func<TIn, TOut> _filter;

        public SelectQuery(AQuery<TIn> query, Func<TIn, TOut> filter) : base(query)
        {
            _filter = filter;
        }

        protected override IEnumerable<TOut> ResolveImpl()
        {
            return Query.ResolveNow().Select(_filter);
        }
    }

    class SelectManyQuery<TIn, TOut> : AQueryDecoratorResolverBase<TIn, TOut>
    {
        readonly Func<TIn, IEnumerable<TOut>> _filter;

        public SelectManyQuery(AQuery<TIn> query, Func<TIn, IEnumerable<TOut>> filter) : base(query)
        {
            _filter = filter;
        }

        protected override IEnumerable<TOut> ResolveImpl()
        {
            return Query.ResolveNow().SelectMany(_filter);
        }
    }
}
