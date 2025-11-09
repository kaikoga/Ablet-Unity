using UnityEngine;

namespace Ablet.Building
{
    public interface IBuildContext
    {
        public IBuildArgument Argument { get; }
        public GameObject CurrentRootObject { get; }
        public Transform CurrentRootTransform { get; }
    }
}
