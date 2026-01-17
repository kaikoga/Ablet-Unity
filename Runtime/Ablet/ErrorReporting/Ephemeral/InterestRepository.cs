using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Ephemeral
{
    class InterestRepository : IDisposable
    {
        public IEnumerable<Object> Objects => _list;

        readonly List<Object> _list = new List<Object>();

        internal void Add(Object interest) => _list.Add(interest);
        internal void Remove(Object interest) => _list.Remove(interest);

        public void Dispose()
        {
            if (_list.Count > 0)
            {
                ErrorReport.LogWarning("InterestScope is not disposed");
            } 
            _list.Clear();
        }
    }
}
