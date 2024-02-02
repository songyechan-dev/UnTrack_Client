using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo_KimJuWan : MonoBehaviour
{
    public enum Info
    {
        REPLAY_05 = 18,
        BACKTOLOBBY_05 = 19,
        REPLAY_06 = 20,
        BACKTOMAIN_06 = 21,
        BACKTOLOBBY_06 = 22,

        GAME_START_01 = 5,
        GAME_EXIT_01 = 6,
        RANKING_01 = 7,
        SETTING_01 = 8,

    }
    public Info myInfo;

}