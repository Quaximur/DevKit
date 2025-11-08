using System;
using R3;

namespace DevKit.Utils
{
    public abstract class ReactiveChangeBase<T> : ReactiveProperty<T>, IReactiveChange
    {
        public bool IsChanged
        {
            get
            {
                if (EqualityComparer != null)
                    return !EqualityComparer.Equals(CachedValue, Value);
                return true;
            }
        }
        public T CachedValue { get; protected set; }
        protected readonly Func<T> _getValue;

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
            OnNext(Value);
        }
    }
}