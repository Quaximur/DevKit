using System;
using UnityEngine;

namespace DevKit.Utils
{
    /// <summary>
    /// Value selection in Unity Inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SelectorAttribute : PropertyAttribute
    {
        public string[] Options { get; }
        public Type SourceType { get; }
        public string SourceMember { get; }

        public SelectorAttribute(params string[] options)
        {
            Options = options;
        }

        public SelectorAttribute(Type sourceType, string sourceMember)
        {
            SourceType = sourceType;
            SourceMember = sourceMember;
        }
    }
}
