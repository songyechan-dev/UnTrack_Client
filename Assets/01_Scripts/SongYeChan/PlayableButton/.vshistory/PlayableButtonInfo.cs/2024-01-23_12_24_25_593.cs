using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo : MonoBehaviour
{
    private enum Info
    {
        GAME_START = 0,
        GAME_FINISH = 1,
        LANKING = 2,
        SETTING = 3
    }
    public Info myInfo;
}
