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

    public void Create_Tr(string objectName, Transform _transform,Transform _parentTransform)
    {
        Vector3 _position = _transform.position;
        Quaternion _rotation = _transform.rotation;
        PhotonNetwork.Instantiate(objectName, _position, _rotation).transform.SetParent(_parentTransform);
    }


    public void Create(string objectName, Vector3 position)
    {
        //플레이어 생성 및 UIManager에 컨트롤러 
        UIManager.Instance().playerController = PhotonNetwork.Instantiate(objectName, position, Quaternion.identity).GetComponent<PlayerController>();
        UIManager.Instance().playerController.transform.localScale = new Vector3(1, 1, 1);
    }
}
