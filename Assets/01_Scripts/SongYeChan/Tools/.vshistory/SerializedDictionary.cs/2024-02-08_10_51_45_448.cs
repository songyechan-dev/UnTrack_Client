using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializedDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // 딕셔너리 형식의 데이터
    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in dictionary)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        if (keys.Count != values.Count)
        {
            throw new Exception($"The number of keys ({keys.Count}) and values ({values.Count}) does not match.");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }
    }
}
