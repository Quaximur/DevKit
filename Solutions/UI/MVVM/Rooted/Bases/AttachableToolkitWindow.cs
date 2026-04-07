using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableToolkitWindow<T> : AttachableToolkitScreen<T> where T : IScreenViewModel
    {
        [SerializeField] protected string _closeButtonName = "CloseButton";
        [SerializeField] protected string _closeBackgroundName = "CloseBackground";

        protected Button _closeButton;
        protected VisualElement _closeBackground;

        protected override void OnInit()
        {
            base.OnInit();

            if (!string.IsNullOrEmpty(_closeButtonName))
                _closeButton = Root.Q<Button>(name: _closeButtonName);

            if (!string.IsNullOrEmpty(_closeBackgroundName))
                _closeBackground = Root.Q<VisualElement>(name: _closeBackgroundName);
        }

        protected override void OnBind(T viewModel)
        {
            base.OnBind(viewModel);
            BindClosingControls();
        }

        /// <summary>
        /// Initializes event subscriptions for close controls (<see cref="_closeButton"/>,
        /// <see cref="_closeBackground"/>)
        /// </summary>
        protected virtual void BindClosingControls()
        {
            _closeButton?.RegisterCallback<ClickEvent>(OnCloseClicked);
            _closeBackground?.RegisterCallback<ClickEvent>(OnCloseClicked);
        }

        protected virtual void OnCloseClicked(ClickEvent _)
        {
            ViewModel.StartClosing();
        }

        public override void Dispose()
        {
            _closeButton?.UnregisterCallback<ClickEvent>(OnCloseClicked);
            _closeBackground?.UnregisterCallback<ClickEvent>(OnCloseClicked);
            base.Dispose();
        }
    }
}