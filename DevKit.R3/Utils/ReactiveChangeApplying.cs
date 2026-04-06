using System;

namespace DevKit.Utils
{
    /// <summary>
    /// <inheritdoc  />
    /// When applying changes, invokes the applyAction.
    /// Also updates the cache, causing the property to appear unchanged.
    /// </summary>
    public class ReactiveChangeApplying<T> : ReactiveChangeBase<T>
    {
        private readonly Action<T> _applyValue;

        public ReactiveChangeApplying(Func<T> getValue, Action<T> applyAction, T initialValue = default) :
            base(getValue, initialValue)
        {
            _applyValue = applyAction;
        }

        public override void ApplyChanges()
        {
            _applyValue(Value);
            UpdateCache();
        }
    }
}