using System;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class ScreenViewModel : BaseViewModel, IScreenViewModel
    {
        public event Action<bool> OnOpenStateChanged
        {
            add
            {
                if (value == null)
                    return;

                OnOpenedSignal += value;
                value(_isOpened);
            }
            remove
            {
                OnOpenedSignal -= value;
            }
        }

        public event Action OnClosingCompleted;

        protected event Action<bool> OnOpenedSignal;

        private bool _isOpened = false;

        public virtual void Open()
        {
            if (_isOpened)
                return;

            _isOpened = true;
            OnOpenedSignal?.Invoke(_isOpened);
        }

        public virtual void StartClosing()
        {
            if (!_isOpened)
                return;
                
            _isOpened = false;
            OnOpenedSignal?.Invoke(_isOpened);
        }

        /// <summary>
        /// Complete closing when animation is finished. Used by View.
        /// </summary>
        public virtual void CompleteClosing()
        {
            OnClosingCompleted?.Invoke();
        }
    }
}