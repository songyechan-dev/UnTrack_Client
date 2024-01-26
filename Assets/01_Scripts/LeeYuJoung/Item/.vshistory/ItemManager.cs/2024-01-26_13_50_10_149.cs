using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    PhotonView pv;
    public enum ITEMTYPE
    {
        WOOD,      // ����
        STEEL,     // ö��
        DROPPEDTRACK,  // ������� ���� Ʈ��
        BUCKET,    // ����
        DYNAMITE,  // ��ź
        AX,        // ����
        PICK       // ���
    }
    public ITEMTYPE itemType;

    private void FixedUpdate()
    {
        if (pv != null && pv.IsMine)
        {
            PhotonTransformView transformView = GetComponent<PhotonTransformView>();
            if (transformView != null)
            {
                transformView.enabled = false;
            }
        }
        else
        {
            PhotonTransformView transformView = GetComponent<PhotonTransformView>();
            if (transformView != null)
            {
                transformView.enabled = true;
            }
        }
    }
}
