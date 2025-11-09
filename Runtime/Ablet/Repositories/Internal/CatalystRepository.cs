using System.Collections.Generic;
using Ablet.API.Internal;
using Ablet.API.Internal.Attributes;

namespace Ablet.Repositories.Internal
{
    class CatalystRepository : DefinitionRepositoryBase<IAbletCatalyst, AbletCatalystAttribute>
    {
        public static readonly CatalystRepository Instance = new CatalystRepository();
        
        public override IEnumerable<IAbletCatalyst> All() => Unordered();
    }
}
