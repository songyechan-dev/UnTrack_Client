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
}
