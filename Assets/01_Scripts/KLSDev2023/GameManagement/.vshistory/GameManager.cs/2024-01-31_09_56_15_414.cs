using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
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
        //GameStart();
        SetKeyCode();
    }

    private void SetKeyCode()
    {
        if (PlayerPrefs.HasKey("PlayerActionKeyCode"))
        {
            KeyCodeInfo.myActionKeyCode = (KeyCode)PlayerPrefs.GetInt("PlayerActionKeyCode");
        }
        else
        {
            KeyCodeInfo.myActionKeyCode = KeyCode.Space;
        }
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
        gameMode = GameMode.Play;
        UIManager.Instance().Init();
    }

    /// <summary>
    /// GameOver시 초기화 및 데이터 저장
    /// </summary>
    public void GameOver()
    {
        gameMode = GameMode.None;
        UIManager.Instance().Init();
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
                UIManager.Instance().Init();
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
        UIManager.Instance().Init();
        Debug.Log("게임끝");
    }

    public void MeterCalculate()
    {
        if (firstFactoriesObject != null && PhotonNetwork.IsMasterClient)
        {
            meter = Mathf.InverseLerp(MapInfo.defaultStartTrackX, MapInfo.finishEndTrackX, firstFactoriesObject.transform.position.x);
            UIManager.Instance().SetText(UIManager.Instance().distance03,(int)(meter *100) +"M");

            object[] data = new object[] { meter };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.METER, data, raiseEventOptions, SendOptions.SendReliable);

        }

    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.METER)
        {
            // 다른 플레이어들이 호출한 RPC로 미터 값을 받음
            object[] receivedData = (object[])photonEvent.CustomData;
            float receivedMeter = (float)receivedData[0];

            UIManager.Instance().SetText(UIManager.Instance().distance03, (int)(receivedMeter * 100) + "m");
        }
    }


    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    #endregion
}
