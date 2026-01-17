using System.Collections.Generic;
using Ablet.API.V1.Querying;

namespace Ablet.Querying.Resolvers
{
    abstract class AQueryResolverBase<T> : AQueryResolver<T>
    {
        public IEnumerable<T> ResolveNow() => ResolveImpl();
        protected abstract IEnumerable<T> ResolveImpl();
    }

    abstract class AQueryDecoratorResolverBase<TIn, TOut> : AQueryResolverBase<TOut>
    {
        protected readonly AQuery<TIn> Query;

        protected AQueryDecoratorResolverBase(AQuery<TIn> query)
        {
            Query = query;
        }
    }

    abstract class ASelectQueryResolverBase<TIn, TOut> : AQueryDecoratorResolverBase<TIn, TOut>
    {
        protected ASelectQueryResolverBase(AQuery<TIn> query) : base(query)
        {
        }

        protected sealed override IEnumerable<TOut> ResolveImpl()
        {
            return Query.ResolveNow(Selector);
        }

        protected abstract IEnumerable<TOut> Selector(TIn item);
    }

}
