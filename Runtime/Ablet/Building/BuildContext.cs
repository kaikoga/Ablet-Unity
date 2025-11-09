using UnityEngine;

namespace Ablet.Building
{
    public class BuildContext
    {
        public readonly BuildArgument Argument;

        public GameObject CurrentRootObject { get; private set; }
        public Transform CurrentRootTransform => CurrentRootObject.transform;

        public void SetCurrentRootObject(GameObject gameObject) => CurrentRootObject = gameObject;

        public BuildContext(BuildArgument argument)
        {
            Argument = argument;
            CurrentRootObject = argument.EntrypointObject;
        }
    }
}
