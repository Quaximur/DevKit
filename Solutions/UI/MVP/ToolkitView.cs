using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.Solutions.UI.MVP
{
    public abstract class ToolkitView : MonoBehaviour, IToolkitView, IDisposable
    {
        public Observable<bool> IsShown => _isShown;
        
        [SerializeField] private VisualTreeAsset _uiAsset;
        private ReactiveProperty<bool> _isShown;

        protected VisualElement Root { get; private set; }

        public VisualElement InitAndGetRoot()
        {
            Init();
            return Root;
        }

        public virtual void Show()
        {
            Root.style.display = DisplayStyle.Flex;
            _isShown.Value = true;
        }

        public virtual void Hide()
        {
            Root.style.display = DisplayStyle.None;
            _isShown.Value = false;
        }

        private void Init()
        {
            Root = _uiAsset.CloneTree();
            Root.pickingMode = PickingMode.Ignore;
            Root.style.flexGrow = 1;
            _isShown = new ReactiveProperty<bool>(Root.style.display == DisplayStyle.Flex);
            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}