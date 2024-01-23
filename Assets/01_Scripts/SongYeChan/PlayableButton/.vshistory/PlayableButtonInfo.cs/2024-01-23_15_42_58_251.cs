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
        SETTING01 = 3
    }
    public Info myInfo;
}
