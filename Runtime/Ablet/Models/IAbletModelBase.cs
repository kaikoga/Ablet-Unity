using System;

namespace Ablet.Models
{
    public interface IAbletModelBase
    {
        Type DefType { get; }
    }
    
    public interface IAbletIdModelBase : IAbletModelBase
    {
        string Id { get; }
        string DisplayName { get; }
    }
}
