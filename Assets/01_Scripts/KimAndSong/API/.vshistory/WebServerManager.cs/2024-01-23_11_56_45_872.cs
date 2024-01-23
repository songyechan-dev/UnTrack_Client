using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebServerManager : MonoBehaviour
{
    private string serverURL = "http://localhost:3000";  // 서버 URL을 적절히 변경하세요

    public IEnumerator Login(string userId, string password)
    {
        // 서버에 보낼 데이터 생성
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("password", password);

        // HTTP POST 요청
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Network error: " + www.error);
            }
            else
            {
                // 서버 응답 처리
                string responseText = www.downloadHandler.text;
                Debug.Log("Server response: " + responseText);

                // JSON 파싱
                JSONNode jsonResponse = JSON.Parse(responseText);

                // success 값을 가져와서 처리
                bool success = jsonResponse["success"].AsBool;
                if (success)
                {
                    Debug.Log("Login successful");
                    // 성공했을 때의 처리
                }
                else
                {
                    Debug.Log("Login failed: " + jsonResponse["message"]);
                    // 실패했을 때의 처리
                }
            }
        }
    }
}
