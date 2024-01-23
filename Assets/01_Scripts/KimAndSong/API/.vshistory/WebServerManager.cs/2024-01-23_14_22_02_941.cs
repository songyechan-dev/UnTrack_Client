using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebServerManager 
{
    private static string serverURL = "http://localhost:3000";  // 서버 URL을 적절히 변경하세요

    public static IEnumerator LoginCoroutine(string userId, string password)
    {
        // 서버에 보낼 데이터 생성
        WWWForm form = new WWWForm();
        form.AddField("user_Id", userId);
        form.AddField("user_password", password);

        // HTTP POST 요청
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL+"/user/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Network error: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Server response: " + responseText);

                JSONNode jsonResponse = JSON.Parse(responseText);

                bool success = jsonResponse["success"].AsBool;
                if (success)
                {
                    Debug.Log("Login successful");
                }
                else
                {
                    Debug.Log("Login failed: " + jsonResponse["message"]);
                }
            }
        }
    }
}
