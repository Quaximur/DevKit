using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UITK
{
    [UxmlElement]
    public abstract partial class ArrowMenu<T> : BaseField<T>
    {
        // USS class names
        public new const string ussClassName = "arrows-menu";
        public const string valueLabelUssClassName = "arrows-menu__value-label";
        public const string arrowButtonUssClassName = "arrows-menu__arrow-button";
        public const string leftArrowButtonUssClassName = "arrows-menu__arrow-button--left";
        public const string rightArrowButtonUssClassName = "arrows-menu__arrow-button--right";

        protected T[] _options = Array.Empty<T>();
        protected int _currentIndex = 0;
        protected string _leftButtonText = "<";
        protected string _rightButtonText = ">";

        protected readonly VisualElement _input;
        protected readonly Label _valueLabel;
        protected readonly Button _leftButton;
        protected readonly Button _rightButton;

        [UxmlAttribute("options")]
        public T[] Options
        {
            get => _options;
            set
            {
                _options = value ?? Array.Empty<T>();
                UpdateOptions();
            }
        }

        [UxmlAttribute("left-button-text")]
        public string LeftButtonText
        {
            get => _leftButtonText;
            set
            {
                _leftButtonText = value;
                if (_leftButton != null)
                    _leftButton.text = value;
            }
        }

        [UxmlAttribute("right-button-text")]
        public string RightButtonText
        {
            get => _rightButtonText;
            set
            {
                _rightButtonText = value;
                if (_rightButton != null)
                    _rightButton.text = value;
            }
        }

        public event Action<int> OnIndexChanged;
        public event Action<T> OnValueChanged;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex == value)
                    return;

                if (Options is { Length: > 0 })
                {
                    _currentIndex = Mathf.Clamp(value, 0, Options.Length - 1);
                    OnIndexChanged?.Invoke(_currentIndex);
                    OnValueChanged?.Invoke(Value);
                }
                else
                {
                    _currentIndex = 0;
                }
                UpdateValueLabel();
            }
        }

        public T Value
        {
            get
            {
                if (_currentIndex >= 0 && _currentIndex < _options.Length)
                    return _options[_currentIndex];
                else
                    return default;
            }
            set
            {
                if (value == null)
                    return;

                var index = Array.IndexOf(_options, value);
                if (index == _currentIndex)
                    return;

                if (index >= 0)
                    CurrentIndex = index;
                else
                {
                    Debug.LogError("No such option");
                    CurrentIndex = 0;
                }
            }
        }

        public ArrowMenu() : this(null)
        {
        }

        public ArrowMenu(string label) : base(label, null)
        {
            _input = this.Q(className: BaseField<bool>.inputUssClassName);
            AddToClassList(ussClassName);

            _leftButton = new Button()
            {
                name = "left-button",
                text = LeftButtonText
            };
            _leftButton.AddToClassList(arrowButtonUssClassName);
            _leftButton.AddToClassList(leftArrowButtonUssClassName);
            _leftButton.RegisterCallback<ClickEvent>(_ => OnArrowButtonClicked(-1));

            _valueLabel = new Label()
            {
                name = "value-label"
            };
            _valueLabel.AddToClassList(valueLabelUssClassName);

            _rightButton = new Button()
            {
                name = "right-button",
                text = RightButtonText
            };
            _rightButton.AddToClassList(arrowButtonUssClassName);
            _rightButton.AddToClassList(rightArrowButtonUssClassName);
            _rightButton.RegisterCallback<ClickEvent>(_ => OnArrowButtonClicked(+1));

            _input.Add(_leftButton);
            _input.Add(_valueLabel);
            _input.Add(_rightButton);

            UpdateValueLabel();
        }

        private void OnArrowButtonClicked(int direction)
        {
            if (Options is { Length: > 0 })
                CurrentIndex = (CurrentIndex + direction + Options.Length) % Options.Length;
        }

        private void UpdateValueLabel()
        {
            if (_valueLabel == null)
                return;

            if (Options is { Length: > 0 } && CurrentIndex >= 0 && CurrentIndex < Options.Length)
                _valueLabel.text = Options[CurrentIndex].ToString();
            else
                _valueLabel.text = string.Empty;
        }

        private void UpdateOptions()
        {
            _currentIndex = 0;
            UpdateValueLabel();
        }
    }
}