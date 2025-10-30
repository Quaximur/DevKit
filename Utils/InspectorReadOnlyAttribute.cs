using System;
using UnityEngine;

namespace Flopin.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InspectorReadOnlyAttribute : PropertyAttribute
    {
    }
}