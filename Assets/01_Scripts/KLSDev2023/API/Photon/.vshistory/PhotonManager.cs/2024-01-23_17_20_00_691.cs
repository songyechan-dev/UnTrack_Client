using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string version = "1.0f";
    private string userID = "hello1";
    private GameObject roomItemPrefab;
    [Header("UI")]

    // 룸 목록에 대한 데이터를 저장하기 위한 딕셔너리 자료형
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    // RoomItem 프리팹이 추가될 ScrollContent
    public Transform scrollContent;

    public TeamManager teamManager;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = DataManager.GetUserID();
        roomItemPrefab = Resources.Load<GameObject>("RoomItem");
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }



  



    #region Callbacks
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
            return;
        }
        else
        {
            //InfoText필요
            //infoText.text = "현재 방 로딩중입니다. \n잠시후 다시 접속해주세요.";
            return;
        }
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

        Guid newUuid = Guid.NewGuid();
        // 룸 생성
        PhotonNetwork.CreateRoom(newUuid.ToString(),ro);
    }

    public override void OnCreatedRoom()
    {

    }

    //유저가 입장시 팀 UI 업데이트
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        teamManager.UpdateTeamUI();
    }

    //유저가 퇴장시 팀 UI 업데이트
    public override void OnPlayerLeftRoom(Player player)
    {
        teamInfoListView.SetActive(false);
        teamManager.Left(player.NickName);
    }


    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (PhotonNetwork.IsConnectedAndReady)
        {
            foreach (var playerInfo in PhotonNetwork.PlayerListOthers)
            {
                if (PhotonNetwork.NickName.Equals(playerInfo.NickName))
                {
                    infoText.text = "동일한 닉네임을 사용중입니다.\n다시 로그인해주세요.";
                    PhotonNetwork.LeaveLobby();
                    PhotonNetwork.LeaveRoom();
                    return;
                }
            }

            // 마스터 클라이언트인 경우에 룸에 입장한 후 전투 씬을 로딩한다.
            if (PhotonNetwork.IsMasterClient)
            {
                gameStartBtn.gameObject.SetActive(true);
                gameStartBtn.onClick.RemoveAllListeners();
                gameStartBtn.onClick.AddListener(() => Onstart(PhotonNetwork.CurrentRoom.Name)); ;
            }

            teamInfoListView.SetActive(true);
            teamManager.TeamSet(PhotonNetwork.NickName);
            if (DBManager.SelectDataPlayer(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName).Rows.Count <= 0)
            {
                DBManager.InsertData(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName, teamManager.teamList[PhotonNetwork.NickName]);
            }
        }
        else
        {
            infoText.text = "현재 방 로딩중입니다. \n잠시후 다시 접속해주세요.";
            return;
        }
    }

    private void Onstart(string roomName)
    {
        PhotonNetwork.LoadLevel("씬이름");
    }

    [PunRPC]
    // 룸 목록을 수신하는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 삭제된 RoomItem 프리팹을 저장할 임시변수
        GameObject tempRoom = null;

        foreach (var roomInfo in roomList)
        {
            // 룸이 삭제된 경우
            if (roomInfo.RemovedFromList == true)
            {
                // 딕셔너리에서 룸 이름으로 검색해 저장된 RoomItem 프리팹를 추출
                rooms.TryGetValue(roomInfo.Name, out tempRoom);

                // RoomItem 프리팹 삭제
                Destroy(tempRoom);

                // 딕셔너리에서 해당 룸 이름의 데이터를 삭제
                rooms.Remove(roomInfo.Name);
                DBManager.DeleteDataAll(roomInfo.Name);
            }
            else // 룸 정보가 변경된 경우
            {
                // 룸 이름이 딕셔너리에 없는 경우 새로 추가
                if (rooms.ContainsKey(roomInfo.Name) == false)
                {
                    // RoomInfo 프리팹을 scrollContent 하위에 생성
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
                    // 룸 정보를 표시하기 위해 RoomInfo 정보 전달
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;
                    roomPrefab.GetComponent<RoomData>().infoText = infoText;

                    // 딕셔너리 자료형에 데이터 추가
                    rooms.Add(roomInfo.Name, roomPrefab);
                    Debug.Log("신입들어옴");
                }
                else
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                    Debug.Log("신입들어옴2");
                }
            }

            Debug.Log($"Room={roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
        }
    }

    public void OnLoginClick()
    {
        // 유저명 저장
        SetUserId();

        // 무작위로 추출한 룸으로 입장

    }
    //public void OnMakeRoomClick()
    //{
    //    // 유저명 저장
    //    SetUserId();

    //    // 룸의 속성 정의
    //    RoomOptions ro = new RoomOptions();
    //    ro.MaxPlayers = 20;     // 룸에 입장할 수 있는 최대 접속자 수
    //    ro.IsOpen = true;       // 룸의 오픈 여부
    //    ro.IsVisible = true;    // 로비에서 룸 목록에 노출시킬 여부

    //    // 룸 생성
    //    PhotonNetwork.CreateRoom(SetRoomName(), ro);
    //}

    #endregion
}
