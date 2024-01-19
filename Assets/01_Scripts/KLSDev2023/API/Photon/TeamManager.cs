using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using UnityEngine;
using UnityEngine.UI;



public class TeamManager : MonoBehaviourPunCallbacks
{
    public string testName;
    public Dictionary<string, string> teamList = new Dictionary<string, string>();
    public GameObject content_Yellow;
    public GameObject content_Blue;
    private GameObject playerItem;

    public List<string> testNum = new List<string>();



    void Start()
    {

        playerItem = Resources.Load<GameObject>("PlayerItem");
        UpdateTeamUI();

    }

    public void TeamSet(string nickName)
    {

        string myTeam = "";
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            myTeam = "Yellow";
        }
        else
        {
            myTeam = "Blue";
        }

        if (!teamList.ContainsKey(nickName))
        {
            photonView.RPC("SyncTeamSet", RpcTarget.AllBuffered, nickName, myTeam);
        }
        UpdateTeamUI();

    }

    [PunRPC]
    void SyncTeamSet(string nickName, string team)
    {
        if (teamList.ContainsKey(nickName))
        {
            teamList[nickName] = team;
        }
        else
        {
            teamList.Add(nickName, team);
        }
        DBManager.DeleteData(PhotonNetwork.CurrentRoom.Name, nickName); // 예를 들어 기존 데이터 삭제
        DBManager.InsertData(PhotonNetwork.CurrentRoom.Name, nickName, team);
        UpdateTeamUI();
        Debug.Log($"내팀은 :::: {nickName} {team}");
    }

    public void Left(string nickName)
    {
        photonView.RPC("TeamListChange", RpcTarget.AllBuffered, nickName);
    }

    [PunRPC]
    void TeamListChange(string nickName)
    {
        teamList.Remove(nickName);
        DBManager.DeleteData(PhotonNetwork.CurrentRoom.Name, nickName);
        UpdateTeamUI();
    }

    public void ChangeTeam(string team)
    {
        photonView.RPC("SyncChangeTeam", RpcTarget.AllBuffered, PhotonNetwork.NickName, team);
        UpdateTeamUI();
    }



    [PunRPC]
    void SyncChangeTeam(string nickName, string team)
    {
        ;
        if (teamList.ContainsKey(nickName))
        {
            teamList[nickName] = team;
        }
        else
        {
            teamList.Add(nickName, team);
        }
        DBManager.DeleteData(PhotonNetwork.CurrentRoom.Name, nickName);
        DBManager.InsertData(PhotonNetwork.CurrentRoom.Name, nickName, team);
        UpdateTeamUI();
    }


    public void UpdateTeamUI()
    {
        if (content_Blue.transform.childCount > 0)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in content_Blue.transform)
            {
                if (child.GetSiblingIndex() != 0)
                {
                    children.Add(child.gameObject);
                }
            }
            foreach (Transform child in content_Yellow.transform)
            {
                if (child.GetSiblingIndex() != 0)
                {
                    children.Add(child.gameObject);
                }
            }

            foreach (GameObject child in children)
            {
                Destroy(child);
            }
        }
        foreach (var player in teamList)
        {
            string playerName = player.Key;
            string playerTeam = player.Value;


            GameObject teamContent = playerTeam.Equals("Yellow") ? content_Yellow : content_Blue;


            GameObject playerUI = Instantiate(playerItem, teamContent.transform);
            playerUI.GetComponentInChildren<Text>().text = playerName;
        }
    }

    public void SetTestName(string nickName)
    {
        testName = nickName;
        photonView.RPC("SetName", RpcTarget.Others, nickName);
    }

    [PunRPC]
    private void SetName(string nickName)
    {
        testName = nickName;
    }

}
