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
    private int needReadyUserCount = 0;
    public PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public int GetNeedReadyUserCount()
    {
        return needReadyUserCount;
    }


    public void SetNeedReadyUserCount(bool isPlus)
    {
        pv.RPC("SetNeedReadyUserCount_Sync", RpcTarget.AllBuffered, isPlus);
    }


    [PunRPC]

    public void SetNeedReadyUserCount_Sync(bool isPlus)
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
    }


}
