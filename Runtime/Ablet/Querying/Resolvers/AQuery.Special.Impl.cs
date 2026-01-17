using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1.Querying;
using Ablet.Models;
using UnityEngine;

namespace Ablet.Querying.Resolvers
{
    class GetEntrypointsQueryResolver : AQueryDecoratorResolverBase<GameObject, (GameObject gameObject, AbletPlatform platform)>
    {
        readonly IEnumerable<AbletPlatform> _platforms;
        readonly bool _includeInactive;
        
        public GetEntrypointsQueryResolver(AQuery<GameObject> query, IEnumerable<AbletPlatform> platforms, bool includeInactive) : base(query)
        {
            _platforms = platforms.ToArray();
            _includeInactive = includeInactive;
        }

        protected override IEnumerable<(GameObject gameObject, AbletPlatform platform)> ResolveImpl()
        {
            // TODO ignore nested
            return Query
                .ResolveNow(gameObject => gameObject.GetComponentsInChildren<Component>(_includeInactive))
                .Where(component => component)
                .Select(component => (component, platform: _platforms.FirstOrDefault(platform => platform.EntrypointComponentType == component.GetType())))
                .Where(cp => cp.platform?.FilterEntrypoint(cp.component) is true)
                .Select(cp => (cp.component.gameObject, cp.platform));
        }
    }
    
    class GetEntrypointForQueryResolver : AQueryDecoratorResolverBase<GameObject, (GameObject gameObject, AbletPlatform platform)>
    {
        readonly IEnumerable<AbletPlatform> _platforms;
        
        public GetEntrypointForQueryResolver(AQuery<GameObject> query, IEnumerable<AbletPlatform> platforms) : base(query)
        {
            _platforms = platforms.ToArray();
        }

        protected override IEnumerable<(GameObject gameObject, AbletPlatform platform)> ResolveImpl()
        {
            // TODO ignore nested
            return Query
                .Select(gameObject => gameObject.GetComponentsInParent<Component>(true)
                    .Where(component => component)
                    .Select(component => (component, platform: _platforms.FirstOrDefault(platform => platform.EntrypointComponentType == component.GetType())))
                    .FirstOrDefault(cp => cp.platform?.FilterEntrypoint(cp.component) is true))
                .Where(cp => cp.component)
                .Select(cp => (cp.component.gameObject, cp.platform))
                .ResolveNow();
        }
    }

}
