using System;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public abstract partial class ToolkitWidget :VisualElement, IView, IDisposable
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
    /// Базовый класс для всех виджетов, использующих UI Toolkit для архитектуры MVVM (Model-View-ViewModel).
    /// Виджеты для UI Toolkit имеют функционал представления, но в отличие от View, наследуются от
    /// VisualElement, а не от MonoBehaviour.
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
