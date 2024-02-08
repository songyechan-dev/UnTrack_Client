using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

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

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var pair in this)
        {
            sb.Append(pair.Key).Append(": ");
            if (pair.Value is List<int[]>)
            {
                List<int[]> list = pair.Value as List<int[]>;
                sb.Append("[");
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append("(").Append(string.Join(", ", list[i])).Append(")");
                    if (i < list.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(pair.Value.ToString());
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
