using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Enum
    public enum GameState 
    {
        GameStart = 0,
        GameClear = 1,
        GameStop = 2,
        GameOver = 3,
        GameEnd = 4
    }
    #endregion

    #region Value
    private int round = 1;
    private int finalRound = 5;
    private float meter;

    private FactoriesObjectManager firstCreatedFactoriesObject;
    public MapCreator mapCreator;
    public GameState gameState;
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
        GameStart();
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
    public float GetMeter()
    {
        return meter;
    }
    #endregion

    #region Setter
    public void SetRound(int _round)
    {
        this.round = _round;
    }
    public void SetMeter(float _meter)
    {
        this.meter = _meter;
    }
    #endregion

    #region Other Func
    /// <summary>
    /// 게임시작시 호출 필요한 함수들 호출
    /// </summary>
    public void GameStart()
    {
        gameState = GameState.GameStart;
        mapCreator.MapLoad();
    }

    /// <summary>
    /// GameOver시 초기화 및 데이터 저장
    /// </summary>
    public void GameOver()
    {
        
    }

    public void GameClear()
    {
        if (!gameState.Equals(GameState.GameClear))
        {
            if (GetRound() < GetFinalRound())
            {
                round++;
                //upgrade 씬호출
                Debug.Log("GameClear");
            }
            else
            {
                GameEnd();
            }
            gameState = GameState.GameClear;
        }
        
    }

    public void GameEnd()
    {
        Debug.Log("게임끝");
    }

    public void MeterCalculate()
    {

        //미터 계산 공식 추가
        //시간초

    }
    #endregion
}
