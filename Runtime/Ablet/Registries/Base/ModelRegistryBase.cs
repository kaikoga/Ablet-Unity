using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.API;
using Ablet.Models;

namespace Ablet.Registries.Base
{
    public abstract class ModelRegistryBase<TKeyType, TModel> : RegistryBase<TKeyType, TModel>
        where TKeyType : IAbletDefinable
        where TModel : class
    {
    }
    
    public abstract class IdModelRegistryBase<TKeyType, TModel> : ModelRegistryBase<TKeyType, TModel>
        where TKeyType : IAbletDefinition
        where TModel : class, IAbletIdModelBase
    {
        public virtual bool TryGetById(string id, [MaybeNullWhen(false)] out TModel value)
        {
            foreach (var val in Unordered().Where(def => def.Id == id))
            {
                value = val;
                return true;
            }
            value = null;
            return false;
        }

        public string ToDisplayName(string id)
        {
            return TryGetById(id, out var value) ? value.DisplayName : id;
        }
    }

}
