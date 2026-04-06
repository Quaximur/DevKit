namespace DevKit.UI
{
    /// <summary>
    /// Template for ALL views.
    /// </summary>
    public interface IView
    {
        public virtual void Init()
        {
        }
        
        public void Show();
        
        public void Hide();
    }
}