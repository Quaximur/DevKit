using System;
using R3;

namespace DevKit.Utils
{
    public abstract class ReactiveChangeBase<T> : ReactiveProperty<T>, IReactiveChange
    {
        public override T Value
        {
            get => base.Value;
            set
            {
                base.Value = value;
                UpdateIsChanged();
            }
        }

        public ReadOnlyReactiveProperty<bool> IsChanged => _isChanged;
        public T CachedValue { get; protected set; }

        protected readonly Func<T> _getValue;
        protected readonly ReactiveProperty<bool> _isChanged = new(false);

        public ReactiveChangeBase(Func<T> getValue, T initialValue = default) :
            base(initialValue)
        {
            _getValue = getValue;
            CachedValue = getValue();
        }

        public abstract void ApplyChanges();
        public void ResetToCached() => Value = CachedValue;

        public void UpdateCache()
        {
            CachedValue = _getValue();
            UpdateIsChanged(); // should be false
        }

        private void UpdateIsChanged()
        {
            if (EqualityComparer != null)
            {
                _isChanged.Value = !EqualityComparer.Equals(CachedValue, Value);
                return;
            }
            FLogger.Log<ReactiveChangeBase<T>>($"EqualityComparer is null");
            _isChanged.Value = true;
        }
    }
}