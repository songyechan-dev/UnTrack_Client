using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo_LeeYuJoung : MonoBehaviour
{
    public enum Info
    {
        CONTINUE_04 = 0,
        ENGINE_UPGRADE_04 = 1,
        STORAGE_UPGRADE_04 = 2,
        MACHINE_UPGRADE_04 = 3,
        PRODUCTIONMACHINE_UPGRADE_04 = 4,
        DYNAMITEMACHINE_UPGRADE_04 = 5,
        WATERTANK_UPGRADE_04 = 6,
        PRODUCTIONMACHINE_BUY_04 = 7,
        DYNAMITEMACHINE_BUY_04 = 8,
        WATERTANK_BUY_04 = 9,
        GAME_EXIT_04 = 10
    }
    public Info myInfo;
    public int machineUpgradeIDX = 0;
}
