using System;

namespace Ablet.Utils.Reflection
{
    static class MaybeActivator
    {
        public static TDefinition? MaybeConstruct<TDefinition>(Type type)
        where TDefinition : class
            => (TDefinition?)type.GetConstructor(Array.Empty<Type>())?.Invoke(null);
    }
}
