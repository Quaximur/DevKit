using System;
using UnityEngine;

namespace DevKit.InterfaceSerialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public readonly Type InterfaceType;

        public RequireInterfaceAttribute(Type interfaceType)
        {
            Debug.Assert(interfaceType.IsInterface, $"{interfaceType} should be an interface type");
            InterfaceType = interfaceType;
        }
    }
}
