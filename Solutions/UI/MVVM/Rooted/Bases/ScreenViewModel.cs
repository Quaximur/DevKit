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

                OnOpenStateChangedSignal += value;
                value(_isOpened);
            }
            remove
            {
                OnOpenStateChangedSignal -= value;
            }
        }

        public event Action OnClosingCompleted;

        protected Action<bool> OnOpenStateChangedSignal;

        protected bool _isOpened = false;

        public virtual void Open()
        {
            if (_isOpened)
                return;

            _isOpened = true;
            OnOpenStateChangedSignal?.Invoke(_isOpened);
        }

        public virtual void StartClosing()
        {
            if (!_isOpened)
                return;
                
            _isOpened = false;
            OnOpenStateChangedSignal?.Invoke(_isOpened);
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