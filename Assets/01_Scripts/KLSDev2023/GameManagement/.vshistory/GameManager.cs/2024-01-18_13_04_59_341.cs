using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Value
    private int round = 1;
    #endregion

    #region Instance
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    }
    #endregion

    #region Default Func
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
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

    #region Other Func
    public void GameStart(int _round)
    {
        SetRound(_round);
    }
    #endregion
}
