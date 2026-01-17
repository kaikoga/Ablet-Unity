using System;

namespace Ablet.Utils
{
    class LazyDisposable : IDisposable
    {
        readonly Func<IDisposable> _generator;

        IDisposable? _generated;

        public LazyDisposable(Func<IDisposable> generator) => _generator = generator;

        public void Materialize()
        {
            _generated ??= _generator();
        }

        public void Dispose()
        {
            _generated?.Dispose();
            _generated = EmptyDisposable.Instance;
        }
    }
}
