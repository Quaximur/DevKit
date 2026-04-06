using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// View using UI Toolkit without binding to a ViewModel.
    /// E.g., used for serialization under a common abstraction.
    /// </summary>
    public abstract class ToolkitView : BaseView, IToolkitView
    {
        /// <summary>
        /// Asset containing the UI for the current View.
        /// </summary>
        [SerializeField] protected VisualTreeAsset _uiAsset;

        /// <summary>
        /// Access to the root element from the VisualTreeAsset _uiAsset.
        /// </summary>
        protected VisualElement Root { get; private set; }

        /// <summary>
        /// View initialization. <br/>
        /// This method must be called before any actions with screen elements (VisualElement),
        /// because the screen will not exist at all before Init() is called.
        /// </summary>
        /// <returns>Root element of the View asset (ToolkitView.Root)</returns>
        public VisualElement InitAndGetRoot()
        {
            Init();
            return Root;
        }

        /// <summary>
        /// Basic UI initialization.
        /// </summary>
        public override sealed void Init()
        {
            Root = UxmlUtil.CloneStyled(_uiAsset);
            ApplyRootStyle(Root);
            OnInit();
        }

        protected virtual void ApplyRootStyle(VisualElement root)
        {
        }

        /// <summary>
        /// Show the screen (call only after initialization).
        /// </summary>
        public override void Show()
        {
            UxmlUtil.Show(Root);
        }

        /// <summary>
        /// Hide the screen (call only after initialization).
        /// </summary>
        public override void Hide()
        {
            UxmlUtil.Hide(Root);
        }

        /// <summary>
        /// Additional custom initialization (called after Init()). <br/>
        /// Here could be gathering of references to various VisualElement via Root.Q.
        /// </summary>
        protected virtual void OnInit()
        {
        }
    }

    /// <summary>
    /// Base class for all Views using UI Toolkit for MVVM (Model-View-ViewModel) architecture.
    /// </summary>
    /// <typeparam name="T">ViewModel with logic for the View</typeparam>
    public abstract class ToolkitView<T> : ToolkitView, IView<T> where T : IViewModel
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