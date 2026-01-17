using System;
using Ablet.API.V1.Querying;
using Ablet.Querying.Resolvers;

namespace Ablet.Querying
{
    public partial class AQueryContext
    {
        public AQuery<T> Return<T>(T value) => Query(new ReturnQueryResolver<T>(value));
        public AQuery<T> Lazy<T>(Func<T> func) => Query(new LazyReturnQueryResolver<T>(func));
    }
}
