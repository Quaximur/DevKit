using System;

namespace DevKit.Utils
{
    /// <summary>
    /// <inheritdoc />
    /// After applying changes, the current value is stored in the cache,
    /// causing the property to appear unchanged.
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