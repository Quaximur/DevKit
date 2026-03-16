using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevKit.InterfaceSerialization
{
    public readonly struct InterfaceArguments
    {
        public readonly Type InterfaceType;
        public readonly Type ObjectType;

        public InterfaceArguments(Type interfaceType, Type objectType)
        {
            Debug.Assert(typeof(Object).IsAssignableFrom(objectType),
                $"{nameof(ObjectType)} should be of type {typeof(Object)}");

            Debug.Assert(interfaceType.IsInterface, $"{nameof(interfaceType)} should be an interface");

            InterfaceType = interfaceType;
            ObjectType = objectType;
        }
    }
}