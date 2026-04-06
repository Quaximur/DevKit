namespace DevKit.Saves
{
    /// <summary>
    /// Example class for saving data with Version.
    /// </summary>
    public abstract class SaveStateBase : IVersioned
    {
        public abstract int Version { get; set; }
    }
}