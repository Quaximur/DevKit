namespace DevKit.UI.MVVM.Bases
{
    public interface IOpenableView : IView
    {
        public void OnOpening();

        public void OnClosing();
    }
}