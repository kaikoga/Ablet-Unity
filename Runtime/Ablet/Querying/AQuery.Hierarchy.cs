using System;
using Ablet.API.V1.Querying;
using Ablet.Querying.Resolvers;
using UnityEngine;

namespace Ablet.Querying
{
    public static partial class AQuery
    {
        public static AQuery<Component> GetComponents(this AQuery<GameObject> query, Type type)
        {
            return query.Create(new GetComponentsQueryResolver(query, type));
        }

        public static AQuery<T> GetComponents<T>(this AQuery<GameObject> query)
        {
            return query.Create(new GetComponentsQueryResolver<T>(query));
        }

        public static AQuery<Component> GetComponentsInChildren(this AQuery<GameObject> query, Type type, bool includeInactive = false)
        {
            return query.Create(new GetComponentsInChildrenQueryResolver(query, type, includeInactive));
        }

        public static AQuery<T> GetComponentsInChildren<T>(this AQuery<GameObject> query, bool includeInactive = false)
        {
            return query.Create(new GetComponentsInChildrenQueryResolver<T>(query, includeInactive));
        }
    }
}
