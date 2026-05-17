using System;
using Ablet.API.V1;
using UnityEngine;

namespace Ablet.Models
{
    public class AbletSubplatform : IAbletIdModelBase
    {
        readonly IAbletSubplatform _def;
        public Type DefType => _def.GetType();

        public string Id => _def.Id;
        public string DisplayName => _def.DisplayName;

        public string PlatformId => _def.PlatformId;
        public int Priority => _def.Priority;
        public bool IsAvailable => _def.IsAvailable;
        public bool IsPreferredSubplatform(GameObject entrypointObject) => _def.IsPreferredSubplatform(entrypointObject);

        public AbletSubplatform(IAbletSubplatform def) => _def = def;
    }
}
