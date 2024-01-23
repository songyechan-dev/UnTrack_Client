using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonObjectCreator : MonoBehaviourPunCallbacks
{
    void Creaate(GameObject _gameObject, Transform _transform)
    {
        Vector3 _position = _transform.position;
        Quaternion _rotation = _transform.rotation;
        PhotonNetwork.Instantiate(_gameObject.transform.tag, _position,_rotation);
    }
}
