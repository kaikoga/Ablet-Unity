using System;
using System.Collections.Generic;
using Ablet.API.V1.Querying;
using UnityEngine;

namespace Ablet.Querying.Resolvers
{
    class GetComponentsQueryResolver : ASelectQueryResolverBase<GameObject, Component>
    {
        readonly Type _type;

        public GetComponentsQueryResolver(AQuery<GameObject> query, Type type)
            : base(query)
        {
            _type = type;
        }

        protected override IEnumerable<Component> Selector(GameObject item) => item.GetComponents(_type);
    }

    class GetComponentsQueryResolver<T> : ASelectQueryResolverBase<GameObject, T>
    {
        public GetComponentsQueryResolver(AQuery<GameObject> query)
            : base(query)
        {
        }

        protected override IEnumerable<T> Selector(GameObject item) => item.GetComponents<T>();
    }

    class GetComponentsInChildrenQueryResolver : ASelectQueryResolverBase<GameObject, Component>
    {
        readonly Type _type;
        readonly bool _includeInactive;

        public GetComponentsInChildrenQueryResolver(AQuery<GameObject> query, Type type, bool includeInactive = false)
            : base(query)
        {
            _type = type;
            _includeInactive = includeInactive;            
        }

        protected override IEnumerable<Component> Selector(GameObject item) => item.GetComponentsInChildren(_type, _includeInactive);
    }

    class GetComponentsInChildrenQueryResolver<T> : ASelectQueryResolverBase<GameObject, T>
    {
        readonly bool _includeInactive;

        public GetComponentsInChildrenQueryResolver(AQuery<GameObject> query, bool includeInactive = false)
            : base(query)
        {
            _includeInactive = includeInactive;            
        }

        protected override IEnumerable<T> Selector(GameObject item) => item.GetComponentsInChildren<T>(_includeInactive);
    }
}
