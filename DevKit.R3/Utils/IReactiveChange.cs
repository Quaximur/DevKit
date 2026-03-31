using R3;

namespace DevKit.Utils
{
    public interface IReactiveChange
    {
        public ReadOnlyReactiveProperty<bool> IsChanged { get; }
        public void ApplyChanges();
        public void ResetToCached();
        public void UpdateCache();
    }
}