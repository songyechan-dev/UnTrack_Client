using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableButtonInfo_KimJuWan : MonoBehaviour
{
    public enum Info
    {
        REPLAY_05 = 0,
        BACKTOLOBBY_05 = 1,
        REPLAY_06 = 2,
        BACKTOMAIN_06 = 3,
        BACKTOLOBBY_06 = 4,
        
    }
    public Info myInfo;
    public int machineUpgradeIDX = 0;
}
