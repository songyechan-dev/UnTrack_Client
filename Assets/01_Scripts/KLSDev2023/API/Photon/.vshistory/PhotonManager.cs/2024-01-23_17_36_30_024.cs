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
    private GameObject roomPrefab;
    [Header("UI")]

    // �� ��Ͽ� ���� �����͸� �����ϱ� ���� ��ųʸ� �ڷ���
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    // RoomItem �������� �߰��� ScrollContent
    public Transform scrollContent;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = DataManager.GetUserID();
        roomPrefab = Resources.Load<GameObject>("RoomItem");
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
            //InfoText�ʿ�
            //infoText.text = "���� �� �ε����Դϴ�. \n����� �ٽ� �������ּ���.";
            return;
        }
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // �뿡 ������ �� �ִ� �ִ� ������ ��
        ro.IsOpen = true;       // ���� ���� ����
        ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� �����ų ����

        Guid newUuid = Guid.NewGuid();
        // �� ����
        PhotonNetwork.CreateRoom(newUuid.ToString(),ro);
    }

    //������ ����� �� UI ������Ʈ
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "����");
    }

    //������ ����� �� UI ������Ʈ
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(player.NickName + "����");
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
                    Debug.Log("����ڰ� �̹� �α��� �� �Դϴ�.\n�ٽ� �α������ּ���.");
                    PhotonNetwork.LeaveLobby();
                    PhotonNetwork.LeaveRoom();
                    return;
                }
            }

            // ������ Ŭ���̾�Ʈ�� ��쿡 �뿡 ������ �� ���� ���� �ε��Ѵ�.
            if (PhotonNetwork.IsMasterClient)
            {
                //���ӽ��� ����
                //gameStartBtn.gameObject.SetActive(true);
                //gameStartBtn.onClick.RemoveAllListeners();
                //gameStartBtn.onClick.AddListener(() => Onstart(PhotonNetwork.CurrentRoom.Name)); ;
            }


        }
        else
        {
            //infoText.text = "���� �� �ε����Դϴ�. \n����� �ٽ� �������ּ���.";
            return;
        }
    }

    private void Onstart(string roomName)
    {
        PhotonNetwork.LoadLevel("���̸�");
    }

    [PunRPC]
    // �� ����� �����ϴ� �ݹ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ������ RoomItem �������� ������ �ӽú���
        GameObject tempRoom = null;

        foreach (var roomInfo in roomList)
        {
            // ���� ������ ���
            if (roomInfo.RemovedFromList == true)
            {
                // ��ųʸ����� �� �̸����� �˻��� ����� RoomItem �����ո� ����
                rooms.TryGetValue(roomInfo.Name, out tempRoom);

                // RoomItem ������ ����
                Destroy(tempRoom);

                // ��ųʸ����� �ش� �� �̸��� �����͸� ����
                rooms.Remove(roomInfo.Name);
                DBManager.DeleteDataAll(roomInfo.Name);
            }
            else // �� ������ ����� ���
            {
                // �� �̸��� ��ųʸ��� ���� ��� ���� �߰�
                if (rooms.ContainsKey(roomInfo.Name) == false)
                {
                    // RoomInfo �������� scrollContent ������ ����
                    GameObject roomPrefab = Instantiate(roomPrefab, scrollContent);
                    // �� ������ ǥ���ϱ� ���� RoomInfo ���� ����
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;
                    //roomPrefab.GetComponent<RoomData>().infoText = infoText;

                    // ��ųʸ� �ڷ����� ������ �߰�
                    rooms.Add(roomInfo.Name, roomPrefab);
                    Debug.Log("���Ե���");
                }
                else
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                    Debug.Log("���Ե���2");
                }
            }

            Debug.Log($"Room={roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
        }
    }


    #endregion
}