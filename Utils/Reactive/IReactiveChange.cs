namespace DevKit.Solutions.Utils
{
    public interface IReactiveChange
    {
        public bool IsChanged { get; }
        public void ApplyChanges();
        public void ResetToCached();
        public void UpdateCache();
    }
}