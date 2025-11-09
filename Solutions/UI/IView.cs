namespace DevKit.UI
{
    /// <summary>
    /// Шаблон для реализации ВСЕХ представлений.
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