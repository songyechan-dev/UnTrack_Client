using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("������ ������");
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        // ���� �����͸� �޴� ���̶�� 
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
