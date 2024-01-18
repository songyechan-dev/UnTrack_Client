using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int round;
    #region Instance
    private static StateManager instance;
    public static StateManager Instance()
    {
        return instance;
    }
    #endregion

    #region Default Func

    #endregion
    #region Getter
    public int GetRound()
    {
        return round;
    }
    #endregion

    #region Setter
    public void SetRound(int _round)
    {
        this.round = _round;
    }
    #endregion
}
