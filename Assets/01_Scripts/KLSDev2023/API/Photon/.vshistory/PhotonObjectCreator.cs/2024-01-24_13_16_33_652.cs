using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonObjectCreator : MonoBehaviourPunCallbacks
{
    public void Create(string objectName, Transform _transform)
    {
        Vector3 _position = _transform.position;
        Quaternion _rotation = _transform.rotation;
        PhotonNetwork.Instantiate(objectName, _position,_rotation);
    }

    public void Create(string objectName, Vector3 position)
    {
        //플레이어 생성 및 UIManager에 컨트롤러 알려주기
        UIManager.Instance().playerController = PhotonNetwork.Instantiate(objectName, position, Quaternion.identity).GetComponent<PlayerController>();
    }
}
