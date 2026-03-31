using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVP
{
    public abstract class ToolkitView : MonoBehaviour, IToolkitView, IDisposable
    {
        [SerializeField] private VisualTreeAsset _uiAsset;

        protected VisualElement Root { get; private set; }

        public VisualElement InitAndGetRoot()
        {
            Init();
            return Root;
        }

        public virtual void Show()
        {
            Root.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            Root.style.display = DisplayStyle.None;
        }

        private void Init()
        {
            Root = _uiAsset.CloneTree();
            Root.pickingMode = PickingMode.Ignore;
            Root.style.flexGrow = 1;
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