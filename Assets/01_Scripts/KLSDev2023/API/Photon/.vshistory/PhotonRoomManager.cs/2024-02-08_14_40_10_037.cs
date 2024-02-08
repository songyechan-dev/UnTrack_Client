using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviour
{
    void RoomClose()
    {
        UIManager.Instance().roomListPanel02.SetActive(false);
    }
}
