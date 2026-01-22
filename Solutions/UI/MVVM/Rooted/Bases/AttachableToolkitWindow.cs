using DevKit.Utils;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableToolkitWindow<T> : AttachableToolkitScreen<T>  where T : IScreenViewModel
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

            _closeButton?.SubscribeCallbackOnce<ClickEvent>(OnCloseClicked)
                .AddTo(_disposables);
            _closeBackground?.SubscribeCallbackOnce<ClickEvent>(OnCloseClicked)
                .AddTo(_disposables);
        }

        protected virtual void OnCloseClicked(ClickEvent _)
        {
            ViewModel.StartClosing();
        }
    }
}