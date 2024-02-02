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

        GAME_START_02 = 4,
        BACK_02 = 5,
        FIND_ROOM_02 = 6,

        CONTINUE_04 = 7,
        ENGINE_UPGRADE_04 = 8,
        STORAGE_UPGRADE_04 = 9,
        MACHINE_UPGRADE_04 = 10,
        PRODUCTIONMACHINE_UPGRADE_04 = 11,
        DYNAMITEMACHINE_UPGRADE_04 = 12,
        WATERTANK_UPGRADE_04 = 13,
        PRODUCTIONMACHINE_BUY_04 = 14,
        DYNAMITEMACHINE_BUY_04 = 15,
        WATERTANK_BUY_04 = 16,
        GAME_EXIT_04 = 17

    }
    public Info myInfo;
    public int machineUpgradeIDX = 0;
}
