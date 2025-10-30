using System;
using UnityEngine;

namespace DevKit.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InspectorReadOnlyAttribute : PropertyAttribute
    {
    }
}