using System.Collections.Generic;
using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.localization;
using Object = UnityEngine.Object;

namespace Ablet.Ndmf
{
    class WrappedError : SimpleError
    {
        public override ErrorSeverity Severity { get; }

        readonly string _message;

        #region unused ndmf API
        public override Localizer? Localizer => null;

        public override string? TitleKey => null;

        public override string[]? TitleSubst => null;
        public override string[]? DetailsSubst => null;
        public override string[]? HintSubst => null;
        #endregion

        public WrappedError(ErrorSeverity errorSeverity, string message, IEnumerable<Object> contexts)
        {
            Severity = errorSeverity;
            _message = message;
            foreach (var context in contexts)
            {
                AddReference(ObjectRegistry.GetReference(context));
            }
        }

        public override string FormatTitle()
        {
            return _message.Split("\n").FirstOrDefault() ?? "";
        }

        public override string FormatDetails()
        {
            return _message;
        }

        public override string FormatHint()
        {
            return "";
        }
    }
}
