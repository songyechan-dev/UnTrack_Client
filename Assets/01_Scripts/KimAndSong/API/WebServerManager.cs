using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebServerManager : MonoBehaviour
{
    public string webUrl = "http://localhost:3000";

    IEnumerator Login()
    {
        // 로그인 요청을 보낼 데이터 (예: 사용자 이름과 비밀번호)
        WWWForm form = new WWWForm();
        form.AddField("user_id", "example_user");
        form.AddField("user_password", "example_password");

        using (UnityWebRequest www = UnityWebRequest.Post(webUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string token = www.downloadHandler.text;
                Debug.Log("Received Token: " + token);

                // 여기에서 토큰을 사용하여 로그인 후의 동작을 수행할 수 있습니다.
            }
        }
    }
}
