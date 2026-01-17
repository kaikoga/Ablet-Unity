using System;

namespace Ablet.Utils
{
    class CompositeDisposable : IDisposable
    {
        readonly IDisposable[] _disposables;
        public CompositeDisposable(params IDisposable[] disposables) => _disposables = disposables;

        public void Dispose()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
        }
    }
}
