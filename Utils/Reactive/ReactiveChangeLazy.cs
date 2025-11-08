using System;

namespace DevKit.Utils
{
    /// <summary>
    /// После применения изменений вызывает действие applyAction.
    /// Также обновляет кэш, из-за чего свойство показывается как неизменённое.
    /// </summary>
    public class ReactiveChangeLazy<T> : ReactiveChangeBase<T>
    {
        private readonly Action<T> _applyValue;

        public ReactiveChangeLazy(Func<T> getValue, Action<T> applyAction, T initialValue = default) :
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