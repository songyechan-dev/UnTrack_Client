using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using UnityEngine;
using UnityEngine.UI;



public class TeamManager : MonoBehaviourPunCallbacks
{
    private string roomName;
    [SerializeField]
    private int needReadyUserCount;
    [SerializeField]
    private int readyUserCount;
    public PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    #region needReadyUserCount

    public int GetNeedReadyUserCount()
    {
        return needReadyUserCount;
    }
    public void SetNeedReadyUserCount(bool isPlus)
    {
        pv.RPC("SetNeedReadyUserCount_Sync", RpcTarget.MasterClient, isPlus);
    }
    [PunRPC]
    public void SetNeedReadyUserCount_Sync(bool isPlus)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isPlus)
            {
                needReadyUserCount++;
            }
            else
            {
                needReadyUserCount--;
            }
            Debug.Log(needReadyUserCount);
            pv.RPC("SyncNeedReadyUserCount", RpcTarget.Others, needReadyUserCount);
        }
    }
    [PunRPC]
    private void SyncNeedReadyUserCount(int updatedValue)
    {
        needReadyUserCount = updatedValue;
        Debug.Log("Synced NeedReadyUserCount: " + needReadyUserCount);
    }

    #endregion

    #region readyUserCount

    public int GetReadyUserCount()
    {
        return readyUserCount;
    }

    public void SetReadyUserCount(bool isPlus)
    {
        pv.RPC("SetReadyUserCount_Sync", RpcTarget.MasterClient, isPlus);
    }

    public void SetReadyUserCount_Sync(bool isPlus)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isPlus)
            {
                readyUserCount++;
            }
            else
            {
                readyUserCount--;
            }
            Debug.Log(readyUserCount);
            pv.RPC("SyncReadyUserCount", RpcTarget.Others, readyUserCount);
        }
    }
    [PunRPC]
    private void SyncReadyUserCount(int updatedValue)
    {
        readyUserCount = updatedValue;
        Debug.Log("readyUserCount: " + readyUserCount);
    }



    #endregion



}
