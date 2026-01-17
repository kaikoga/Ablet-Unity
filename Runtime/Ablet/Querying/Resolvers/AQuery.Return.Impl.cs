using System;
using System.Collections.Generic;

namespace Ablet.Querying.Resolvers
{
    class ReturnQueryResolver<T> : AQueryResolverBase<T>
    {
        readonly T _value;

        public ReturnQueryResolver(T value)
        {
            _value = value;
        }

        protected override IEnumerable<T> ResolveImpl()
        {
            yield return _value;
        }
    }

    class LazyReturnQueryResolver<T> : AQueryResolverBase<T>
    {
        readonly Func<T> _func;

        public LazyReturnQueryResolver(Func<T> func)
        {
            _func = func;
        }

        protected override IEnumerable<T> ResolveImpl()
        {
            yield return _func();
        }
    }
}
