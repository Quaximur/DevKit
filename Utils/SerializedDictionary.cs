using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevKit.Utils
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] public List<TKey> keys = new List<TKey>();
        [SerializeField] public List<TValue> values = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => TransferSerializedKeys();
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        public void TransferSerializedKeys()
        {
            Clear();

            for (var i = 0; i < Math.Min(keys.Count, values.Count); i++)
            {
                this[keys[i]] = values[i];
            }
        }
    }
}