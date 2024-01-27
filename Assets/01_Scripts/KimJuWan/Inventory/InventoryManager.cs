using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviourPun
{
    public ItemManager.ITEMTYPE itemType;
    public int itemNum = 0;

    public bool SaveInventory(GameObject _item)
    {
        ItemManager.ITEMTYPE _itemType = _item.GetComponent<ItemManager>().itemType;

        if (itemNum <= 0)
        {
            _item.layer = LayerMask.NameToLayer("PickSlot");
            itemType = _itemType;
            itemNum += 1;
            return true;
        }
        else
        {
            if (itemType == _itemType && itemNum < 4)
            {
                _item.layer = LayerMask.NameToLayer("PickSlot");
                itemNum += 1;
                return true;
            }
            else
            {
                Debug.Log("같은 아이템만 들어 올릴 수 있음...");
                return false;
            }
        }
    }

    // 플레이어 손에서 아이템 내려 놓음
    public void OutInventory()
    {
        itemNum = 0;
    }

    // 플레이어 손에서 아이템 내려 놓음
    public void OutInventory(GameObject _item)
    {
        _item.layer = LayerMask.NameToLayer("Default");
        itemNum = 0;
    }

    // 바닥에 있는 아이템 더미에 올림
    public bool DroppedSlotIn(GameObject _item)
    {
        ItemManager.ITEMTYPE _itemType = _item.GetComponent<ItemManager>().itemType;

        if (itemNum <= 0)
        {
            _item.layer = LayerMask.NameToLayer("PickSlot");
            itemType = _itemType;
            itemNum += 1;
            return true;
        }
        else
        {
            if (itemType == _itemType)
            {
                _item.layer = LayerMask.NameToLayer("PickSlot");
                itemNum += 1;
                return true;
            }
            else
            {
                Debug.Log("같은 아이템만 들어 올릴 수 있음...");
                return false;
            }
        }
    }

    // 바닥에 있는 아이템 더미를 다시 들어 올림
    public void DroppedSlotOut()
    {
        if (itemNum > 0)
        {
            itemNum -= 1;
        }
    }
}