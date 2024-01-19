using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager instance;
    public static GameManager Instance()
    {
        return this.instance;
    }
    #endregion
    private int round;


    #region Default Func
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
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
