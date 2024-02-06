using ExitGames.Client.Photon;
using LeeYuJoung;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField]
    private int round = 1;
    private int finalRound = 5;
    private float meter;



    public GameObject factoriesObjectPrefab;
    public MapCreator mapCreator;
    public GameState gameState;
    public GameMode gameMode;
    public TimeManager timeManager;
    public GameObject firstFactoriesObject;
    public Transform myPlayer;
    private UnityEngine.AsyncOperation asyncOperation;
    public List<bool> playerReadyStates = new List<bool>();
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
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        gameState = GameState.GameStart;
        gameMode = GameMode.Play;
        
        object[] data = new object[] { (int)gameState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.GAME_MODE, data, raiseEventOptions, SendOptions.SendReliable);
        
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
                gameState = GameState.GameClear;
                gameMode = GameMode.None;
                object[] data = new object[] { (int)gameState };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.GAME_MODE, data, raiseEventOptions, SendOptions.SendReliable);
                StartCoroutine(LoadSceneAsync(4));
            }
            else
            {
                GameEnd();
            }
        }
    }

    IEnumerator LoadSceneAsync(int sceneNum)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneNum);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        GameInIt();
    }

    public void GameInIt()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            UIManager.Instance().voltNumText04.text = StateManager.Instance().voltNum.ToString();

            UIManager.Instance().playerController = PhotonNetwork.Instantiate("Player", new Vector3(0,20,0), Quaternion.identity).GetComponent<PlayerController>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            QuestManager.Instance().CheckCompletion();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    TeamManager teamManager = GameObject.Find("TeamManager").GetComponent<TeamManager>();
                    teamManager.needReadyUserCount = PhotonNetwork.CurrentRoom.PlayerCount;
                    myPlayer = players[i].transform;
                    myPlayer.GetComponent<PlayerController>().teamManager = null;
                    myPlayer.GetComponent<PlayerController>().teamManager = teamManager;
                    FactoriesObjectCreator.Instance().Init();       
                }
            }
            

        }
    }

    public void GameEnd()
    {
        gameState = GameState.GameEnd;
        gameMode = GameMode.None;
        //UIManager.Instance().Init();
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
        if (photonEvent.Code == (int)SendDataInfo.Info.GAME_MODE)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            GameState _gameState = (GameState)((int)receivedData[0]);
            if (_gameState.Equals(GameState.GameStart))
            {
                SetGameMode(GameMode.Play,GameState.GameStart);
            }
            if (_gameState.Equals(GameState.GameClear))
            {
                round++;
                gameState = GameState.GameClear;
                gameMode = GameMode.None;
                StartCoroutine(LoadSceneAsync(4));
            }
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

    public void SetGameMode(GameMode _gameMode, GameState _gameState)
    {
        gameMode = _gameMode;
        gameState = _gameState;
    }


    public IEnumerator SleepCoroutine(float seconds, Action _action)
    {
        yield return new WaitForSeconds(seconds);
        _action();
    }


    #endregion
}
