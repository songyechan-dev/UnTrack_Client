using Photon.Pun;
using Photon.Realtime;
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
            Debug.Log("여기호출");
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
                    Debug.Log("게임시작");
                    PhotonNetwork.LoadLevel(3);
                }
                else
                {
                    readyUserCount++;
                }
            }
            else
            {
                //게임시작 후 벗어나면 게임시작 진행 멈추는 기능 필요 할 수도 있음(게임이 바로 시작되는게아니라 스테이가 필요하다면)
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

    //TODO : 트러블 슈팅 - 송예찬
    int leftCount = 0;
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Player 태그를 가진 오브젝트를 찾아서 삭제
            GameObject[] playersWithTag = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject playerObject in playersWithTag)
            {
                PhotonView pv = playerObject.GetComponent<PhotonView>();
                if (pv.CreatorActorNr == otherPlayer.ActorNumber)
                {
                    PhotonNetwork.Destroy(playerObject);
                }
            }
            SetReadyUserCount(false);
        }
    }

}
