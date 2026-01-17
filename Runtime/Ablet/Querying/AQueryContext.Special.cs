using System.Collections.Generic;
using Ablet.API.V1.Querying;
using Ablet.Models;
using Ablet.Querying.Resolvers;
using UnityEngine;

namespace Ablet.Querying
{
    public partial class AQueryContext
    {
        public AQuery<(GameObject gameObject, AbletPlatform platform)> GetSceneEntrypoints(IEnumerable<AbletPlatform> platforms, bool includeInactive)
        {
            return Query(new GetEntrypointsQueryResolver(GetRootGameObjects(), platforms, includeInactive));
        }
    }
}
