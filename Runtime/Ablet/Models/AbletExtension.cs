using System;
using Ablet.API.V1;

namespace Ablet.Models
{
    class AbletExtension : IAbletModelBase
    {
        public readonly IAbletExtension Def;

        public Type DefType => Def.GetType();
        public Type ForType => Def.ForType;

        public AbletExtension(IAbletExtension def) => Def = def;
    }
}
