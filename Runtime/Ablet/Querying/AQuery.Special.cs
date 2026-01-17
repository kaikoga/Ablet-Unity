using System.Collections.Generic;
using Ablet.API.V1.Querying;
using Ablet.Models;
using Ablet.Querying.Resolvers;
using UnityEngine;

namespace Ablet.Querying
{
    public static partial class AQuery
    {
        public static AQuery<(GameObject gameObject, AbletPlatform platform)> GetEntrypoints(this AQuery<GameObject> query, IEnumerable<AbletPlatform> platforms, bool includeInactive)
        {
            return query.Create(new GetEntrypointsQueryResolver(query, platforms, includeInactive));
        }

        public static AQuery<(GameObject gameObject, AbletPlatform platform)> GetEntrypointFor(this AQuery<GameObject> query, IEnumerable<AbletPlatform> platforms)
        {
            return query.Create(new GetEntrypointForQueryResolver(query, platforms));
        }

    }
}
