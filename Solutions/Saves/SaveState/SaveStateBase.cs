namespace DevKit.Saves
{
    /// <summary>
    /// Общий класс для различных версий состояний приложения.
    /// </summary>
    public abstract class SaveStateBase : IVersioned
    {
        public abstract int Version { get; set; }
    }
}