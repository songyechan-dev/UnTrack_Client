using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSyncManager : MonoBehaviourPun
{
    public void SetParent(Transform _tr)
    {
        if (_tr.GetComponent<PhotonView>() != null)
        {
            transform.parent = _tr;
            int viewId = _tr.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetParent_Sync", RpcTarget.OthersBuffered, viewId);
        }
        else if (_tr.GetComponentInParent<PhotonView>() != null)
        {
            transform.parent = _tr;
            int viewId = _tr.GetComponentInParent<PhotonView>().ViewID;
            photonView.RPC("SetParent_Sync", RpcTarget.OthersBuffered, viewId);
        }
    }

    public void SetPosition(Vector3 _targetPos)
    {
        transform.position = _targetPos;
        int viewId = GetComponent<PhotonView>().ViewID;
        photonView.RPC("SetPosition_Sync", RpcTarget.Others, viewId,_targetPos);
    }




    [PunRPC]
    void SetParent_Sync(int viewId)
    {
        PhotonView parentPhotonView = PhotonView.Find(viewId);
        if (parentPhotonView != null)
        {
            transform.parent = parentPhotonView.transform;
        }
    }

    [PunRPC]
    void SetParent_Sync(int viewId,Vector3 _targetPos)
    {
        PhotonView parentPhotonView = PhotonView.Find(viewId);
        if (parentPhotonView != null)
        {
            transform.position = _targetPos;
        }
    }

    public void DroppedSlotIn(Transform _droppedSlot, GameObject _targetGameObject)
    {
        if (_droppedSlot.GetComponent<PhotonView>() != null)
        {
            int droppedSlotViewId = _droppedSlot.GetComponent<PhotonView>().ViewID;
            int targetGameObjectViewId = _targetGameObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("DroppedSlotIn_Sync", RpcTarget.All, droppedSlotViewId, targetGameObjectViewId);
        }
    }

    [PunRPC]
    public void DroppedSlotIn_Sync(int droppedSlotViewId, int targetGameObjectViewId)
    {
        PhotonView droppedSlotPhotonView = PhotonView.Find(droppedSlotViewId);
        PhotonView targetGameObjectPhotonVIew = PhotonView.Find(targetGameObjectViewId);
        if (droppedSlotPhotonView != null && targetGameObjectPhotonVIew != null)
        {
            droppedSlotPhotonView.transform.GetComponent<InventoryManager>().DroppedSlotIn(targetGameObjectPhotonVIew.gameObject);
        }
    }


    public void DestroyObject()
    {
        photonView.RPC("DestroyObject_Sync", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyObject_Sync()
    {
        Debug.Log("ªË¡¶1!!!!!!!");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    
}
