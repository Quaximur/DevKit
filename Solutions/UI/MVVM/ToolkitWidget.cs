using System;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Widgets for UI Toolkit have view functionality, but unlike View, they inherit from
    /// VisualElement rather than MonoBehaviour.
    /// </summary>
    public abstract partial class ToolkitWidget : VisualElement, IView, IDisposable
    {
        protected VisualElement Root => this;

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public virtual void Show()
        {
            style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            style.display = DisplayStyle.None;
        }

        public virtual void Dispose()
        {
        }
    }

    /// <summary>
    /// Base class for all widgets using UI Toolkit for MVVM (Model-View-ViewModel) architecture. <br/>
    /// <inheritdoc/>
    /// </summary>
    [UxmlElement]
    public abstract partial class ToolkitWidget<T> : ToolkitWidget, IView<T> where T : IViewModel
    {
        protected T ViewModel { get; private set; }

        public void Bind(T viewModel)
        {
            ViewModel = viewModel;
            OnBind(viewModel);
        }

        protected abstract void OnBind(T viewModel);
    }
}
