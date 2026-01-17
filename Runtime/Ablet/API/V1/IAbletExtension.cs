using System;

namespace Ablet.API.V1
{
    public interface IAbletExtension : IAbletDefinable
    {
        Type ForType { get; }
    }
}
