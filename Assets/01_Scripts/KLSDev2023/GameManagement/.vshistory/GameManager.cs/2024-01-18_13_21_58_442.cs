using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Value
    private int round = 1;
    private int finalRound = 5;
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
    public int GetFinalRound()
    {
        return finalRound;
    }
    #endregion

    #region Setter
    public void SetRound(int _round)
    {
        this.round = _round;
    }
    public void SetFinalRound(int _round)
    {
        this.finalRound = _round;
    }
    #endregion

    #region Other Func
    /// <summary>
    /// 게임시작시 호출 필요한 함수들 호출
    /// </summary>
    /// <param name="_round">진행할 라운드</param>
    public void GameStart(int _round)
    {
        SetRound(_round);
        MapCreator.Instance().Create();

    }

    /// <summary>
    /// GameOver시 초기화 및 데이터 저장
    /// </summary>
    public void GameOver()
    {
    
    }
    #endregion
}
