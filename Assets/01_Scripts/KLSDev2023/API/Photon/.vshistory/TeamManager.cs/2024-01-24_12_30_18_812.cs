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
    private int needReadyUserCount = 0

    PhotonView.RPC("SetNeedReadyUserCount", RpcTarget.Others, false);

}
