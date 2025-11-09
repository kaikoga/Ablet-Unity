using System.Collections.Generic;
using System.Linq;
using Ablet.Repositories;
using UnityEngine;

namespace Ablet.Querying
{
    public class GetEntrypointsQuery : AMultipleQuery<GameObject>
    {
        public override IEnumerable<GameObject> Query()
        {
            var entrypointComponentTypes = PlatformRepository.Instance.EntrypointComponentTypes;

            return new GetRootGameObjectsQuery().Query()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<Component>(true))
                .Where(component => component && entrypointComponentTypes.Contains(component.GetType()))
                .Select(component => component.gameObject);
        }
    }
}
