using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // stream - 데이터를 주고 받는 통로 
        // 내가 데이터를 보내는 중이라면
        if (stream.IsWriting)
        {
            // 이 방안에 있는 모든 사용자에게 브로드캐스트 
            // - 내 포지션 값을 보내보자
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        // 내가 데이터를 받는 중이라면 
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
