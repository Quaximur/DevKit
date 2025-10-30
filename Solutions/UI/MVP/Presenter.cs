namespace DevKit.UI.MVP
{
    public abstract class Presenter<TView> where TView : IView
    {
        protected TView _view;

        public void Bind(TView view)
        {
            _view = view;
            OnBind(view);
        }

        protected virtual void OnBind(TView view)
        {
        }
    }
}