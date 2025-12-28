using UnityEngine;
using System.Collections.Generic;
using System;

namespace DevKit.Utils
{
    [Serializable]
    public class InspectorReadonlyList<T> : List<T>
    {
        [SerializeField] private List<T> _lockedItems;

        public InspectorReadonlyList() : base()
        {
            _lockedItems = this;
        }
        
        public InspectorReadonlyList(int capacity) : base(capacity)
        {
            _lockedItems = this;
        }

        public InspectorReadonlyList(IEnumerable<T> values) : base(values)
        {
            _lockedItems = this;
        }
    }
}