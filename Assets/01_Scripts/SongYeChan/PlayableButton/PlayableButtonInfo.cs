using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo : MonoBehaviour
{
    public enum Info
    {
        GAME_START = 0,
        GAME_EXIT = 1,
        RANKING = 2,
        SETTING = 3,
        FIND_ROOM_02 = 4,
        BACK_TO_MAIN_02 = 5,
        GAME_START_02 = 6,
    }
    public Info myInfo;
}
