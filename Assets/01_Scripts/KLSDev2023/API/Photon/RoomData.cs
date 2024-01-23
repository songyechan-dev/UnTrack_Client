//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine.UI;

//public class RoomData : MonoBehaviour
//{
//    private RoomInfo _roomInfo;
//    // ������ �ִ� TMP_Text�� ������ ����
//    private Text roomInfoText;
//    // PhotonManager ���� ����
//    private PhotonManager photonManager;
//    public Text infoText;
//    // ������Ƽ ����
//    public RoomInfo RoomInfo
//    {
//        get
//        {
//            return _roomInfo;
//        }
//        set
//        {
//            _roomInfo = value;
//            // �� ���� ǥ��
//            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
//            // ��ư Ŭ�� �̺�Ʈ�� �Լ� ����
//            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
//        }
//    }

//    void Awake()
//    {
//        roomInfoText = GetComponentInChildren<Text>();
//        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
//    }

//    void OnEnterRoom(string roomName)
//    {
//        if (PhotonNetwork.IsConnected == false)
//        {
//            PhotonNetwork.ConnectUsingSettings();
//            return;
//        }
//        if (!PhotonNetwork.IsConnectedAndReady)
//        {
//            return;
//        }
//        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name == roomName)
//        {
//            PhotonNetwork.LeaveLobby();
//            PhotonNetwork.LeaveRoom();
//            infoText.text = "���� �ش�濡 �������Դϴ�.";
//            return;
//        }
//        if (PhotonNetwork.InRoom)
//        {
//            PhotonNetwork.LeaveLobby();
//            PhotonNetwork.LeaveRoom();
//            PhotonNetwork.Reconnect();
//            return;
//        }

//        //photonManager.SetUserId();
//        infoText.text = "";        // �� ����
//        PhotonNetwork.JoinRoom(roomName);
//    }
//}