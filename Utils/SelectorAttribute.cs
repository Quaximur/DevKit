using System;
using UnityEngine;

namespace Flofin.Utils
{
    /// <summary>
    /// Позволяет выбрать значение из заданного набора строк.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SelectorAttribute : PropertyAttribute
    {
        public string[] Options { get; }
        public Type SourceType { get; }
        public string SourceMember { get; }

        /// <summary>
        /// Получает конкретный массив значений.
        /// </summary>
        public SelectorAttribute(params string[] options)
        {
            Options = options;
        }

        /// <summary>
        /// Получает тип и имя статического массива/свойства, из которого нужно взять варианты.
        /// </summary>
        public SelectorAttribute(Type sourceType, string sourceMember)
        {
            SourceType = sourceType;
            SourceMember = sourceMember;
        }
    }
}
