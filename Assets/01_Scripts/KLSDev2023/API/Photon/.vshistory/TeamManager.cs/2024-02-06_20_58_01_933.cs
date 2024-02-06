using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class TeamManager : MonoBehaviourPunCallbacks
{
    private string roomName;
    [SerializeField]
    public int needReadyUserCount;
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
        if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            pv.RPC("SetNeedReadyUserCount_Sync", RpcTarget.MasterClient, isPlus);
        }
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
            pv.RPC("SyncNeedReadyUserCount", RpcTarget.Others, needReadyUserCount);
        }
    }
    [PunRPC]
    private void SyncNeedReadyUserCount(int updatedValue)
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            needReadyUserCount = updatedValue;
            Debug.Log("Synced NeedReadyUserCount: " + needReadyUserCount);
        }
        else
        {
            Debug.Log("����ȣ��");
        }
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

    [PunRPC]
    public void SetReadyUserCount_Sync(bool isPlus)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isPlus)
            {
                if (GetReadyUserCount() == GetNeedReadyUserCount() - 1 && GetNeedReadyUserCount() >1)
                {
                    readyUserCount++;
                    Debug.Log("���ӽ���");
                    PhotonNetwork.LoadLevel(3);
                    return;
                }
                else
                {
                    readyUserCount++;
                }
            }
            else
            {
                //���ӽ��� �� ����� ���ӽ��� ���� ���ߴ� ��� �ʿ� �� ���� ����(������ �ٷ� ���۵Ǵ°Ծƴ϶� �����̰� �ʿ��ϴٸ�)
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

    //TODO : Ʈ���� ���� - �ۿ���
    int exitedPlayerActNo = -1;
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient && exitedPlayerActNo != otherPlayer.ActorNumber)
        {
            exitedPlayerActNo = otherPlayer.ActorNumber;
            Hashtable playerCustomProperties = otherPlayer.CustomProperties;

            SetReadyUserCount(false);
            SetNeedReadyUserCount(false);
        }
    }

}
