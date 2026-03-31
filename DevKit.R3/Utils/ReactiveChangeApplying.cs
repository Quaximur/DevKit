using System;

namespace DevKit.Utils
{
    /// <summary>
    /// При применении изменений вызывает действие applyAction.
    /// Также обновляет кеш, из-за чего свойство показывается как неизменённое.
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