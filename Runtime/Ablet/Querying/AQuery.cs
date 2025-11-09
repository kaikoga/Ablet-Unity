using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ablet.Querying
{
    public static class AQuery
    {
        public static AMultipleQuery<Component> GetComponents(Type type, bool includeInactive = false) => new GetComponentsQuery(type, includeInactive);
        public static AMultipleQuery<T> GetComponents<T>(bool includeInactive = false) => new GetComponentsQuery<T>(includeInactive);
        public static AMultipleQuery<GameObject> GetEntrypoints() => new GetEntrypointsQuery();
    }

    public abstract class AQuery<T>
    {
        public abstract T Query();
    }

    public abstract class AMultipleQuery<T> : AQuery<IEnumerable<T>>
    {
    }
}
