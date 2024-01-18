using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int round;
    #region Instance, Default Func

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
