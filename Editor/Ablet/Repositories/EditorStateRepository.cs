using System;

namespace Ablet.Repositories
{
    public class EditorStateRepository
    {
        public static readonly EditorStateRepository Instance = new EditorStateRepository();

        public event Action? OnChanged;

        bool _isEnhancedInplacePreview;

        public bool IsEnhancedInplacePreview
        {
            get => _isEnhancedInplacePreview;
            set
            {
                _isEnhancedInplacePreview = value;
                OnChanged?.Invoke();
            }
        }
    }
}
