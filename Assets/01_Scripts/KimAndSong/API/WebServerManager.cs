using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
}
