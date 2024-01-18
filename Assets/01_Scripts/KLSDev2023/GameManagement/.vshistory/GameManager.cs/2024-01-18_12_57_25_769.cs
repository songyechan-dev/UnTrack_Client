using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static StateManager instance;
    public static StateManager Instance()
    {
        return instance;
    }
    #endregion
    private int round;


    #region Default Func
    private void Awake()
    {
        
    }
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
