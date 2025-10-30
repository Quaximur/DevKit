namespace Inui.Core.Providers.SaveState
{
    /// <summary>
    /// Общий класс для различных версий состояний приложения.
    /// </summary>
    public abstract class SaveStateBase : IVersioned
    {
        public abstract int Version { get; set; }
    }
}