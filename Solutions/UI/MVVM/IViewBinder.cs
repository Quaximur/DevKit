namespace DevKit.UI.MVVM
{
    public interface IViewBinder<TViewModel> where TViewModel : IViewModel
    {
        public TViewModel Open();
        public void Close();
    }
}