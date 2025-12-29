using System;

namespace DevKit.Utils
{
    /// <summary>
    /// После применения изменений текущее значение заносится в кеш, 
    /// из-за чего свойство показывается ка неизменённое. 
    /// </summary>
    public class ReactiveChange<T> : ReactiveChangeBase<T>
    {
        public ReactiveChange(Func<T> getValue, T initialValue = default) :
            base(getValue, initialValue)
        {
        }

        public override void ApplyChanges() => UpdateCache();
    }
}