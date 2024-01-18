using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public class InventoryManager : MonoBehaviour
{
    public Stack<GameObject> items = new Stack<GameObject>();
    
    public void SaveInventory(GameObject item)
    {
        item.layer = LayerMask.NameToLayer("PickSlot");

        if (items.Count > 0)
            item.transform.position = items.Peek().transform.position + Vector3.up;
        if (items.Count <= 0)
            item.transform.position = transform.position;
        
        items.Push(item);
    }

    public void OutInventory()
    {
        if (items.Count > 0)
            items.Peek().layer = LayerMask.NameToLayer("Default");
        items.Clear();
        
    }

    public void DroppedSlotIn(GameObject _item)
    {
        Debug.Log("::: 바닥에 내려 놓았습니다 :::");
        _item.layer = LayerMask.NameToLayer("PickSlot");
        _item.tag = "DroppedSlot";

        if(items.Count > 0)
            _item.transform.position = items.Peek().transform.position + Vector3.up;
        
        items.Push(_item);
    }

    public GameObject DroppedSlotout()
    {
        return items.Pop();
    }
}

    






