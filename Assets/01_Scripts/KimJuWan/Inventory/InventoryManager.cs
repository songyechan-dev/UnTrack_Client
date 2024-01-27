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
                Debug.Log("���� �����۸� ��� �ø� �� ����...");
                return false;
            }
        }
    }

    // �÷��̾� �տ��� ������ ���� ����
    public void OutInventory()
    {
        itemNum = 0;
    }

    // �÷��̾� �տ��� ������ ���� ����
    public void OutInventory(GameObject _item)
    {
        _item.layer = LayerMask.NameToLayer("Default");
        itemNum = 0;
    }

    // �ٴڿ� �ִ� ������ ���̿� �ø�
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
                Debug.Log("���� �����۸� ��� �ø� �� ����...");
                return false;
            }
        }
    }

    // �ٴڿ� �ִ� ������ ���̸� �ٽ� ��� �ø�
    public void DroppedSlotOut()
    {
        if (itemNum > 0)
        {
            itemNum -= 1;
        }
    }
}