using UnityEngine.UIElements;

namespace DevKit.UITK
{
    [UxmlElement]
    public partial class KitSliderInt : SliderInt
    {
        /// <summary>
        /// Включить/отключить текстовое поле с текущим значением (только для чтения)
        /// </summary>
        [UxmlAttribute]
        public bool ShowReadonlyValue
        {
            get => _showReadonlyValue;
            set
            {
                if (value)
                {
                    if (_readonlyValueText == null)
                    {
                        _readonlyValueText = new TextElement();
                        _readonlyValueText.AddToClassList("kit-slider__readonly-value");
                        _readonlyValueText.text = this.value.ToString();
                        RegisterCallback<ChangeEvent<int>>(SetValueText);
                        _readonlyValueText.RegisterCallback<AttachToPanelEvent>(_ =>
                            _readonlyValueText.text = this.value.ToString());
                        _inputContainer.Add(_readonlyValueText);
                    }
                }
                else
                {
                    if (_readonlyValueText != null)
                    {
                        UnregisterCallback<ChangeEvent<int>>(SetValueText);
                        _inputContainer.Remove(_readonlyValueText);
                        _readonlyValueText = null;
                    }
                }

                _showReadonlyValue = value;
            }
        }

        private readonly VisualElement _inputContainer;
        private TextElement _readonlyValueText;
        private bool _showReadonlyValue = false;

        public KitSliderInt() : base(null)
        {
            AddToClassList("kit-slider");
            _inputContainer = this.Q(className: inputUssClassName);
        }

        private void SetValueText(ChangeEvent<int> evt)
        {
            _readonlyValueText.text = evt.newValue.ToString();
        }
    }
}
