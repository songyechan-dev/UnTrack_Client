using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using System.Data;
using UnityEngine.UI;

public class TeamInfo : MonoBehaviourPun
{
    public Dictionary<string, string> teamList = new Dictionary<string, string>();
    public Dictionary<string, int> killCount = new Dictionary<string, int>();
    public Dictionary<string, int> myKillCount = new Dictionary<string, int>();
    public Dictionary<string, int> myDeathCount = new Dictionary<string, int>();

    public Text blueKill;
    public Text yellowKill;
    public void SetKillCount(string nickName, string team, string type)
    {
        photonView.RPC("SyncKillCount", RpcTarget.AllBuffered, nickName, team, type);
    }

    [PunRPC]
    void SyncKillCount(string nickName, string team, string type)
    {
        if (!killCount.ContainsKey("Yellow"))
        {
            killCount.Add("Yellow", 0);
        }
        else if (!killCount.ContainsKey("Blue"))
        {
            killCount.Add("Blue", 0);
        }
        if (type.Equals("kill"))
        {
            killCount[team] = killCount[team] + 1;
        }
        if (!myKillCount.ContainsKey(nickName))
        {
            myKillCount.Add(nickName, 0);
            myDeathCount.Add(nickName, 0);
        }
        else
        {
            if (type.Equals(killCount)) myKillCount[nickName] = myKillCount[nickName] + 1;
            else myDeathCount[nickName] = myKillCount[nickName] + 1;
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        if (killCount.ContainsKey("Yellow"))
        {
            yellowKill.text = killCount["Yellow"].ToString();
        }
        if (killCount.ContainsKey("Blue"))
        {
            blueKill.text = killCount["Blue"].ToString();
        }


    }

    public void SetTeamList()
    {
        photonView.RPC("SyncTeamList", RpcTarget.AllBuffered);
    }
    public void RemoveTeamList()
    {
        photonView.RPC("SyncRemoveTeam", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        UpdateUI();
    }

    [PunRPC]
    void SyncRemoveTeam(string nickName)
    {
        if (teamList.ContainsKey(nickName))
        {
            teamList.Remove(nickName);
        }
        if (myKillCount.ContainsKey(nickName))
        {
            myKillCount.Remove(nickName);
        }
        if (myDeathCount.ContainsKey(nickName))
        {
            myDeathCount.Remove(nickName);
        }
        DBManager.DeleteData(PhotonNetwork.CurrentRoom.Name, nickName);
    }




    [PunRPC]
    private void SyncTeamList()
    {
        DataTable data = DBManager.SelectData(PhotonNetwork.CurrentRoom.Name);

        if (data != null && data.Rows.Count > 0)
        {
            foreach (DataRow row in data.Rows)
            {
                string roomName = row["roomName"].ToString();
                string nickName = row["nickName"].ToString();
                string team = row["team"].ToString();
                if (!teamList.ContainsKey(nickName))
                {
                    teamList.Add(nickName, team);
                }

            }
        }
        if (killCount.Count > 0)
        {
            UpdateUI();
        }
    }
}


