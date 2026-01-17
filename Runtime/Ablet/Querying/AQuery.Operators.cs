using System;
using System.Collections.Generic;
using Ablet.API.V1.Querying;
using Ablet.Querying.Resolvers;

namespace Ablet.Querying
{
    public static partial class AQuery
    {
        public static AQuery<T> Where<T>(this AQuery<T> query, Func<T, bool> filter)
        {
            return query.Create(new WhereQuery<T>(query, filter));
        }
        
        public static AQuery<TOut> Select<TIn, TOut>(this AQuery<TIn> query, Func<TIn, TOut> filter)
        {
            return query.Create(new SelectQuery<TIn, TOut>(query, filter));
        }

        public static AQuery<TOut> SelectMany<TIn, TOut>(this AQuery<TIn> query, Func<TIn, IEnumerable<TOut>> filter)
        {
            return query.Create(new SelectManyQuery<TIn, TOut>(query, filter));
        }
    }
}
