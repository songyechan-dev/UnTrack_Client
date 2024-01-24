using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo : MonoBehaviour
{
    public enum Info
    {
        GAME_START_01 = 0,
        GAME_EXIT_01 = 1,
        RANKING_01 = 2,
        SETTING_01 = 3,
        GAME_START_02 = 
    }
    public Info myInfo;
}
