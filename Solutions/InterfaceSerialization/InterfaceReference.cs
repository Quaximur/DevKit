using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevKit.InterfaceSerialization
{
    [Serializable]
    public class InterfaceReference<TInterface, TObject>
        where TInterface : class
        where TObject : Object
    {
        [SerializeField, HideInInspector] private TObject _underlyingValue;

        public TInterface Value
        {
            get
            {
                if (_underlyingValue == null)
                    return null;

                if (_underlyingValue is TInterface @interface)
                    return @interface;

                throw new InvalidOperationException($"{_underlyingValue} should implement interface {typeof(TInterface)}");
            }

            set
            {
                if (value == null)
                {
                    _underlyingValue = null;
                    return;
                }

                if (value is not TInterface @interface)
                    throw new ArgumentException($"{value} should implement interface {typeof(TInterface)}");

                if (@interface is not TObject @object)
                    throw new ArgumentException($"{value} should be of type {typeof(TObject)}");

                _underlyingValue = @object;
            }
        }

        public TObject UnderlyingValue
        {
            get => _underlyingValue;
            set => _underlyingValue = value;
        }

        public InterfaceReference()
        {
        }

        public InterfaceReference(TInterface @interface)
        {
            _underlyingValue = @interface as TObject;
        }

        public InterfaceReference(TObject target)
        {
            _underlyingValue = target;
        }
    }

    [Serializable]
    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object>
        where TInterface : class
    {
    }
}