using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ITEMTYPE
    {
        WOOD,      // ����
        STEEL,     // ö��
        BUCKET,    // ����
        DYNAMITE,  // ��ź
        AX,        // ����
        PICK       // ���
    }
    public ITEMTYPE itemType;
}
