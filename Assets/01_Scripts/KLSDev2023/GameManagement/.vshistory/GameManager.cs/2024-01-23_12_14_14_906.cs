using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Enum
    public enum GameState 
    {
        GameStart = 0,
        GameStop = 1,
        GameClear = 2,
        GameOver = 3,
        GameEnd = 4
    }
    public enum GameMode
    {
        None = 0,
        Play = 1
    }
    #endregion

    #region Value
    private int round = 1;
    private int finalRound = 5;
    private float meter;



    public GameObject factoriesObjectPrefab;
    public MapCreator mapCreator;
    public GameState gameState;
    public GameMode gameMode;
    public TimeManager timeManager;
    public GameObject firstFactoriesObject;
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

    private void Update()
    {
        if (gameState.Equals(GameState.GameStart))
        {
            MeterCalculate();
        }
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
        MapCreator.Instance().MapLoad();
        gameMode = GameMode.Play;
    }

    /// <summary>
    /// GameOver시 초기화 및 데이터 저장
    /// </summary>
    public void GameOver()
    {
        gameMode = GameMode.None;
    }

    public void GameClear()
    {
        if (gameState < GameState.GameClear)
        {
            if (GetRound() < GetFinalRound())
            {
                round++;
                //upgrade 씬호출
                Debug.Log("GameClear");
                gameState = GameState.GameClear;
                gameMode = GameMode.None;
            }
            else
            {
                GameEnd();
            }
        }
        
    }

    public void GameEnd()
    {
        gameState = GameState.GameEnd;
        gameMode = GameMode.None;
        Debug.Log("게임끝");
    }

    public void MeterCalculate()
    {
        if (firstFactoriesObject != null)
        {
            meter = Mathf.InverseLerp(MapInfo.defaultStartTrackX, MapInfo.finishEndTrackX, firstFactoriesObject.transform.position.x);
            //Debug.Log((int)(meter *100) +"미터");
        }

    }
    #endregion
}
