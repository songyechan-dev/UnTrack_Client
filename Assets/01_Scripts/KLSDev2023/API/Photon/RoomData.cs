//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine.UI;

//public class RoomData : MonoBehaviour
//{
//    private RoomInfo _roomInfo;
//    // 하위에 있는 TMP_Text를 저장할 변수
//    private Text roomInfoText;
//    // PhotonManager 접근 변수
//    private PhotonManager photonManager;
//    public Text infoText;
//    // 프로퍼티 정의
//    public RoomInfo RoomInfo
//    {
//        get
//        {
//            return _roomInfo;
//        }
//        set
//        {
//            _roomInfo = value;
//            // 룸 정보 표시
//            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
//            // 버튼 클릭 이벤트에 함수 연결
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
//            infoText.text = "현재 해당방에 접속중입니다.";
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
//        infoText.text = "";        // 룸 접속
//        PhotonNetwork.JoinRoom(roomName);
//    }
//}