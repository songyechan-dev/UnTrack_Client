using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
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
}
