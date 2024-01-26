using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, IPunObservable
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
        if (stream.IsWriting)
        {
            Debug.Log("데이터 쓰는중");
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
