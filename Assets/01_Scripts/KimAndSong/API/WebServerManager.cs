using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class RankingList
{
    public List<TeamData> lankingList;
}
[Serializable]
public class TeamData
{
    public string teamName;
    public float total_clearTime;
    public float round1_clearTime;
    public float round2_clearTime;
    public float round3_clearTime;
    public float round4_clearTime;
    public float round5_clearTime;
    public string player_1;
    public string player_2;
    public string player_3;
    public string player_4;

}
public static class WebServerManager 
{
    private static string serverURL = "http://211.47.91.114:3000";  // 서버 URL을 적절히 변경하세요
    
    
    public static IEnumerator LoginCoroutine(string userId, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", password);
        Debug.Log("호출됨");
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL+"/user/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UIManager.Instance().ActiveAndDeActive(UIManager.Instance().loginFailPanel01, UIManager.Instance().loginPanel01);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Server response: " + responseText);

                JSONNode jsonResponse = JSON.Parse(responseText);

                bool success = jsonResponse["success"].AsBool;
                if (success)
                {
                    DataManager.SetUserID(userId);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    UIManager.Instance().ActiveAndDeActive(UIManager.Instance().loginFailPanel01, UIManager.Instance().loginPanel01);
                }
            }
        }
    }

    public static IEnumerator RankingPanelCoroutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(serverURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonData = www.downloadHandler.text;
                RankingList rankingList = JsonUtility.FromJson<RankingList>(jsonData);

                if (rankingList != null && rankingList.lankingList != null)
                {


                    foreach (TeamData teamData in rankingList.lankingList)
                    {

                        GameObject bar = UIManager.Instance().CreateBar();
                        bar.transform.SetParent(GameObject.Find("RankingContent").transform);
                        bar.transform.localScale = Vector3.one;
                        bar.transform.GetChild(0).GetComponent<Text>().text = $"{rankingList.lankingList.IndexOf(teamData) + 1}."; // 순위
                        bar.transform.GetChild(1).GetComponent<Text>().text = teamData.teamName; // 팀 이름
                        bar.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.total_clearTime / 60, teamData.total_clearTime % 60); // 총 클리어 시간
                        bar.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.round1_clearTime / 60, teamData.round1_clearTime % 60); // 라운드 1 클리어 시간
                        bar.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.round2_clearTime / 60, teamData.round2_clearTime % 60); // 라운드 2 클리어 시간
                        bar.transform.GetChild(9).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.round3_clearTime / 60, teamData.round3_clearTime % 60); // 라운드 3 클리어 시간
                        bar.transform.GetChild(10).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.round4_clearTime / 60, teamData.round4_clearTime % 60); // 라운드 4 클리어 시간
                        bar.transform.GetChild(11).GetChild(0).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", teamData.round5_clearTime / 60, teamData.round5_clearTime % 60); // 라운드 5 클리어 시간
                        bar.transform.GetChild(3).GetComponent<Text>().text = teamData.player_1; // 플레이어 1
                        bar.transform.GetChild(4).GetComponent<Text>().text = teamData.player_2; // 플레이어 2
                        bar.transform.GetChild(5).GetComponent<Text>().text = teamData.player_3; // 플레이어 3
                        bar.transform.GetChild(6).GetComponent<Text>().text = teamData.player_4; // 플레이어 4

                    }
                }

            }
            www.Dispose();
        }
    }

    public static IEnumerator InsertDataCoroutine(string teamName, float total_clearTime, float round1_clearTime, float round2_clearTime, float round3_clearTime, float round4_clearTime,
            float round5_clearTime,List<string>players)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamName", teamName);
        form.AddField("total_clearTime", total_clearTime.ToString());
        form.AddField("round1_clearTime", round1_clearTime.ToString());
        form.AddField("round2_clearTime", round2_clearTime.ToString());
        form.AddField("round3_clearTime", round3_clearTime.ToString());
        form.AddField("round4_clearTime", round4_clearTime.ToString());
        form.AddField("round5_clearTime", round5_clearTime.ToString());
        for (int i = 0; i < players.Count; i++)
        {
            form.AddField("player_" + (i + 1), players[i]);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/rank", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseTxt = www.downloadHandler.text;

                JSONNode dataResponse = JSON.Parse(responseTxt);

                bool success = dataResponse["success"].AsBool;
                if (success)
                {
                    yield return null;
                }

            }
        }
    }
}


