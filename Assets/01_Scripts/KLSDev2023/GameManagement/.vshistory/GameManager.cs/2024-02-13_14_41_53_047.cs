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
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static FactoryManager;

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

    [SerializeField]
    private int derailmentCount = 0;
    private int maxDederailmentCount = 6;

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
        //if (round != 1)
        //{
            StateManager.Instance().productionMachines.Clear();
            StateManager.Instance().dynamiteMachines.Clear();
            StateManager.Instance().waterTanks.Clear();
            StateManager.Instance().BringFactoryValue();
        //}
        Debug.Log("갯수" +StateManager.Instance().factorys["ProductionMachine"][0][1]);
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
        StateManager.Instance().currentTime = 0f;
        gameMode = GameMode.None;
        gameState = GameState.GameOver;
        TimeManager.Instance().roundClearTimeList.Clear();
        TimeManager.Instance().PrevTime = TimeManager.Instance().CurTime;
        
        object[] data = new object[] { (int)gameState };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.GAME_MODE, data, raiseEventOptions, SendOptions.SendReliable);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(5);
        }
    }

    public void GameClear()
    {
        if (gameState < GameState.GameClear)
        {
            float time = TimeManager.Instance().GetCurTime();
            TimeManager.Instance().roundClearTimeList.Add(time);
            if (GetRound() < GetFinalRound())
            {
                StateManager.Instance().currentTime= 0f;
                maxDederailmentCount = 0;
                round++;
                gameState = GameState.GameClear;
                gameMode = GameMode.None;
                object[] data = new object[] { (int)gameState, time };
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

    public void GameExit()
    {
        maxDederailmentCount = 0;
        if (StateManager.Instance() != null)
        {
            StateManager.Instance().currentTime= 0f;
            StateManager.Instance().voltNum = 0;
            StateManager.Instance().engineMaxVolume = 5;
            StateManager.Instance().engineCurrentVolume = 4;

            StateManager.Instance().storageMaxVolume = 10;
            if (StateManager.Instance().storages.ContainsKey("WOOD"))
            {
                StateManager.Instance().storages["WOOD"] = 0;
            }
            else
            {
                StateManager.Instance().storages.Add("WOOD", 0);
            }
            if (StateManager.Instance().storages.ContainsKey("STEEL"))
            {
                StateManager.Instance().storages["STEEL"] = 0;
            }
            else
            {
                StateManager.Instance().storages.Add("STEEL", 0);
            }
            StateManager.Instance().factorys = new Dictionary<string, List<int[]>>() { { "ProductionMachine", new List<int[]> { new int[] { 0, 5 } } }, { "WaterTank", new List<int[]> { new int[] { 0, 5 } } }, { "DynamiteMachine", new List<int[]>() } };
            StateManager.Instance().engineUpgradePrice = 3;
            StateManager.Instance().storageUpgradePrice = 2;
            StateManager.Instance().factoryPrice = new Dictionary<string, List<int>>() { { "ProductionMachine", new List<int> { 1 } }, { "WaterTank", new List<int> { 1 } }, { "DynamiteMachine", new List<int> { 1 } } };
            StateManager.Instance().machineAddPrice = new Dictionary<string, int>() { { "ProductionMachine", 2 }, { "WaterTank", 2 }, { "DynamiteMachine", 2 } };
            StateManager.Instance().currentTime = 0.0f;
            StateManager.Instance().currentTime= 0f;
            
            StateManager.Instance().productionMachines.Clear();
            StateManager.Instance().dynamiteMachines.Clear();
            StateManager.Instance().waterTanks.Clear();
        }

        if (GameManager.Instance() != null)
        {
            GameManager.Instance().SetRound(1);
        }
        if (TimeManager.Instance() != null)
        {
            TimeManager.Instance().roundClearTimeList.Clear();
            TimeManager.Instance().finalTime = 0;
        }
    }


    public void GameEnd()
    {
        gameState = GameState.GameEnd;
        gameMode = GameMode.None;

        float time = TimeManager.Instance().GetCurTime();
        TimeManager.Instance().roundClearTimeList.Add(time);
        for (int i = 0; i < TimeManager.Instance().roundClearTimeList.Count; i++)
        {
            TimeManager.Instance().finalTime += TimeManager.Instance().roundClearTimeList[i];
        }
        StateManager.Instance().productionMachines.Clear();
        StateManager.Instance().dynamiteMachines.Clear();
        StateManager.Instance().waterTanks.Clear();
        StateManager.Instance().BringFactoryValue();
        object[] data = new object[] { (int)gameState, time, TimeManager.Instance().finalTime };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.GAME_MODE, data, raiseEventOptions, SendOptions.SendReliable);
        
        //UIManager.Instance().Init();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(6);
        }
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


    public void SetDerailmentCount()
    {
        derailmentCount++;
        if (derailmentCount == maxDederailmentCount)
        {
            derailmentCount = 0;
            GameOver();
        }
        object[] data = new object[] { derailmentCount };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.DERAILMENT_COUNT, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public int GetDerailmentCount()
    {
        return derailmentCount;
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
                //if (round != 1)
                //{
                    StateManager.Instance().productionMachines.Clear();
                    StateManager.Instance().dynamiteMachines.Clear();
                    StateManager.Instance().waterTanks.Clear();
                    StateManager.Instance().BringFactoryValue();
                //}
            }
            if (_gameState.Equals(GameState.GameClear))
            {
                TimeManager.Instance().roundClearTimeList.Add((float)receivedData[1]);
                round++;
                gameState = GameState.GameClear;
                gameMode = GameMode.None;
                StateManager.Instance().currentTime= 0f;
                maxDederailmentCount = 0;
                StartCoroutine(LoadSceneAsync(4));
            }
            if (_gameState.Equals(GameState.GameOver))
            {
                StateManager.Instance().currentTime = 0f;
                TimeManager.Instance().roundClearTimeList.Clear();
                TimeManager.Instance().PrevTime = TimeManager.Instance().CurTime;
                gameMode = GameMode.None;
                gameState = GameState.GameOver;
            }
            if (_gameState.Equals(GameState.GameEnd))
            {
                gameState = GameState.GameEnd;
                gameMode = GameMode.None;
                StateManager.Instance().productionMachines.Clear();
                StateManager.Instance().dynamiteMachines.Clear();
                StateManager.Instance().waterTanks.Clear();
                StateManager.Instance().BringFactoryValue();
                TimeManager.Instance().roundClearTimeList.Add((float)receivedData[1]);
                for (int i = 0; i < TimeManager.Instance().roundClearTimeList.Count; i++)
                {
                    TimeManager.Instance().finalTime = (float)receivedData[2];
                }

                
            }

        }
        if (photonEvent.Code == (int)SendDataInfo.Info.DERAILMENT_COUNT)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            derailmentCount = (int)receivedData[0];
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
