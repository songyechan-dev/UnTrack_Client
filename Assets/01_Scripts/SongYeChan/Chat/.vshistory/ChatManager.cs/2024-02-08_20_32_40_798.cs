using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ChatManager : MonoBehaviourPun
{
    public GameObject player;
    public float hideTime = 5.0f;
    private Coroutine hideCoroutine;

    private void Start()
    {
        UIManager.Instance().chat.transform.Find("Chat_Btn").GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance().chat.transform.Find("Chat_Btn").GetComponent<Button>().onClick.AddListener(ChatBtnOnClick);
        UIManager.Instance().chat.transform.Find("Chat_Text").GetComponent<InputField>().onSubmit.AddListener(ChatBtnOnClick);
    }

    public void ChatBtnOnClick(string value)
    {
        string text = value;
        player.transform.Find("Chat_Text").GetComponent<TextMeshPro>().text = text;
.
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(WaitAndHideChat());

        object[] data = new object[] { text, player.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.CHAT, data, raiseEventOptions, SendOptions.SendReliable);
        UIManager.Instance().chat.transform.Find("Chat_Text").GetComponent<InputField>().text = "";
    }

    private IEnumerator WaitAndHideChat()
    {
        yield return new WaitForSeconds(hideTime);
        HideChat();
    }

    public void HideChat()
    {
        player.transform.Find("Chat_Text").GetComponent<TextMeshPro>().text = "";
        object[] data = new object[] { player.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.CHAT_HIDE, data, raiseEventOptions, SendOptions.SendReliable);
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.CHAT)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            string text = (string)receivedData[0];
            int ViewID = (int)receivedData[1];

            GameObject _otherPlayer = PhotonView.Find(ViewID).gameObject;
            _otherPlayer.transform.Find("Chat_Text").GetComponent<TextMeshPro>().text = text;
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.CHAT_HIDE)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int ViewID = (int)receivedData[0];

            GameObject _otherPlayer = PhotonView.Find(ViewID).gameObject;
            _otherPlayer.transform.Find("Chat_Text").GetComponent<TextMeshPro>().text = "";
        }
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
