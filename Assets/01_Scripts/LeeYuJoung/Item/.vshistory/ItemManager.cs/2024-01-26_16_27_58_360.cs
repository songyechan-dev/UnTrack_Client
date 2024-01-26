using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviourPunCallbacks
{
    public enum ITEMTYPE
    {
        WOOD,      // 목재
        STEEL,     // 철재
        DROPPEDTRACK,  // 연결되지 않은 트랙
        BUCKET,    // 물통
        DYNAMITE,  // 폭탄
        AX,        // 도끼
        PICK       // 곡괭이
    }
    public ITEMTYPE itemType;

    public override void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // 모든 요청에 대해 승인
        targetView.TransferOwnership(requestingPlayer);
    }
}
