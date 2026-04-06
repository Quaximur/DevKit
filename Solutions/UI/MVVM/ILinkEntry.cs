namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Contract for a link for opening a window.
    /// </summary>
    public interface ILinkEntry
    {
        public void Open();
        public void Close();
    }
}