namespace DevKit.UI.MVVM.Bases
{
    public interface IScreenAttach<T> : IView<T>, IAttachableView where T : IScreenViewModel
    {
    }
}