using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviourPunCallbacks
{
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

    public override void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // ��� ��û�� ���� ����
        targetView.TransferOwnership(requestingPlayer);
    }
}
