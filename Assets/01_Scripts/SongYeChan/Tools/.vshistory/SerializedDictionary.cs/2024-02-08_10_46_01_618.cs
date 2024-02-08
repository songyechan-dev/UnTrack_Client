using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // Before serialization, clear the lists and add all elements from the dictionary
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // After deserialization, rebuild the dictionary using the lists
    public void OnAfterDeserialize()
    {
        Clear();
        if (keys.Count != values.Count)
        {
            throw new Exception($"The number of keys ({keys.Count}) and values ({values.Count}) does not match.");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}
