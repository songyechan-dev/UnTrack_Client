using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<List<TValue>> values = new List<List<TValue>>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        if (keys.Count != values.Count)
        {
            throw new Exception($"Error!!");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}
