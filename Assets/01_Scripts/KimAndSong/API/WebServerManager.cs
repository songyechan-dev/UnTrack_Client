using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public static class WebServerManager 
{
    private static string serverURL = "http://localhost:3000";  // 서버 URL을 적절히 변경하세요

    
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
                var rankingData = JSON.Parse(www.downloadHandler.text);
                Debug.Log(rankingData);
                Debug.Log(www.downloadHandler.text);
                for (int i = 0; i < rankingData.Count; i++)
                {

                    GameObject bar = UIManager.Instance().CreateBar();
                    bar.transform.SetParent(GameObject.Find("RankingContent").transform);
                    bar.transform.localScale = Vector3.one;
                    bar.transform.GetChild(0).GetComponent<Text>().text = $"{i + 1}.";
                    bar.transform.GetChild(1).GetComponent<Text>().text = rankingData[i]["teamName"];
                    bar.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = rankingData[i]["total_clearTime"];
                    bar.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = rankingData[i]["round1_clearTime"];
                    bar.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = rankingData[i]["round2_clearTime"];
                    bar.transform.GetChild(9).GetChild(0).GetComponent<Text>().text = rankingData[i]["round3_clearTime"];
                    bar.transform.GetChild(10).GetChild(0).GetComponent<Text>().text = rankingData[i]["round4_clearTime"];
                    bar.transform.GetChild(11).GetChild(0).GetComponent<Text>().text = rankingData[i]["round5_clearTime"];
                    bar.transform.GetChild(3).GetComponent<Text>().text = rankingData[i]["player_1"];
                    bar.transform.GetChild(4).GetComponent<Text>().text = rankingData[i]["player_2"];
                    bar.transform.GetChild(5).GetComponent<Text>().text = rankingData[i]["player_3"];
                    bar.transform.GetChild(6).GetComponent<Text>().text = rankingData[i]["player_4"];
                    


                }
            }
            else
            {
                Debug.Log(www.result);
                Debug.Log(www.downloadHandler.text);
            }

           
            www.Dispose();
        }
    }

}
