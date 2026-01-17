using System;

namespace Ablet.Utils
{
    class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance = new EmptyDisposable();

        public void Dispose() { }
    }
}
