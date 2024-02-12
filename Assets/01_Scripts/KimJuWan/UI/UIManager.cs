using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GameManager;
using LeeYuJoung;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.Playables;
using System;
using System.Linq;
using UnityEngine.Rendering;

public enum closeBtnType
{
    ROOMLIST01_CLOSE =0,
}

public class UIManager : MonoBehaviour
{
    #region Instance
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }
    #endregion
    public PlayerController playerController;
    public GameObject canvas;
    public GameObject ground;
    public GameObject rankingBarPrefab;
    public string keySet = "PlayerActionKeyCode";
    public Action OnSpaceKeyPress;


    #region Scene00
    public GameObject loadPanel;
    public PlayableDirector clip;
    #endregion

    #region Scene01
    [Header("PlayableButtons")]

    public GameObject playableButton_GameStart01;
    public GameObject playableButton_Ranking01;
    public GameObject playableButton_GameExit01;
    public GameObject playableButton_Setting01;

    [Header("Panel")]
    public GameObject loginPanel01;
    public GameObject settingPanel01;
    public GameObject loginFailPanel01;
    public GameObject rankingPanel01;

    [Header("Text")]
    public Text keySettingText01;



    #endregion

    #region Scene02
    public GameObject tempPlayer;
    [Header("PlayableButtons")]
    public GameObject playableButton_GameStart02;
    public GameObject playableButton_Back02;
    public GameObject playableButton_FindRoom02;
    [Header("Panel")]
    public GameObject roomListPanel02;
    public GameObject loginFailPanel02;
    [Header("Text")]
    public TextMeshPro roomIdText02;
    #endregion

    #region Scene03
    [Header("Panel")]
    public GameObject rDVindex03;
    public GameObject speedIndex03;
    public GameObject questIndex03;
    public GameObject chat;

    [Header("Text")]
    public Text distance03;
    public Text volt03;
    #endregion

    #region Scene04
    public UpgradeManager upgradeManager;
    public Transform[] pos = new Transform[9];
    public List<GameObject> productionMachineList = new List<GameObject>();
    public List<GameObject> dynamiteMachineList = new List<GameObject>();
    public List<GameObject> waterTankList = new List<GameObject>();

    [Header("PlayableButtons")]
    public GameObject playableButton_CONTINUE_04;
    public GameObject playableButton_ENGINE_UPGRADE_04;
    public GameObject playableButton_STORAGE_UPGRADE_04;
    public GameObject playableButton_MACHINE_UPGRADE_04;
    public GameObject playableButton_PRODUCTIONMACHINE_BUY_04;
    public GameObject playableButton_DYNAMITEMACHINE_BUY_04;
    public GameObject playableButton_WATERTANK_BUY_04;
    public GameObject playableButton_GAME_EXIT_04;
    [Header("Panel")]
    public GameObject machineUpgradePanel04;
    public GameObject engineDesPanel04;
    public GameObject storageDesPanel04;
    public GameObject RDVIndex04;
    public GameObject speedIndex04;
    [Header("Text")]
    public Text voltNumText04;
    public TextMeshPro enginePriceText04;
    public TextMeshPro engineDesText04;
    public TextMeshPro storagePriceText04;
    public TextMeshPro storageDesText04;
    public TextMeshPro productionMachineBuyPriceText04;
    public TextMeshPro dynamiteMachineBuyPriceText04;
    public TextMeshPro waterTankBuyPriceText04;
    #endregion

    #region Scene05
    [Header("PlayableButtons")]
    public GameObject playableButton_ReStart05;
    public GameObject playableButton_Back05;
    [Header("Text")]
    public TextMeshPro roundText05;
    public TextMeshPro gameOverTimeText05;
    #endregion

    #region Scene06
    [Header("PlayableButtons")]
    public GameObject playableButton_ReStart06;
    public GameObject playableButton_BackToMain06;
    public GameObject playableButton_BackToLobby06;
    [Header("Text")]
    public TextMeshPro clearTimeText06;
    public TextMeshPro storageLvText06;
    public TextMeshPro dynamiteLvText06;
    public TextMeshPro productionLvText06;
    public TextMeshPro watertankLvText06;
    #endregion

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
        Init();
        
        
        
        
    }

    
    #region Scene00
    public void StartLoading()
    {
        Invoke("EndLoading", (float)clip.duration);

        AudioManager.Instnce().PlayBGM(SceneManager.GetActiveScene().buildIndex);
        loadPanel.SetActive(false);
        clip.Play();
    }

    public void EndLoading()
    {
        SceneManager.LoadScene(1);
        AudioManager.Instnce().PlayBGM(1);
    }
    #endregion

    /// <summary>
    /// 플레이어가 해당 버튼에 머물때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    
    public void PlayAbleButton_OnStay(PlayableButtonInfo.Info _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo.Info.GAME_START_01:
                Debug.Log("여기실행");
                ActiveAndDeActive(loginPanel01, ground);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            case PlayableButtonInfo.Info.GAME_EXIT_01:
                PlayerPrefs.Save();
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                Application.Quit();
                break;
            case PlayableButtonInfo.Info.RANKING_01:
                GameObject player = GameObject.FindWithTag("Player");
                Destroy(player);
                rankingPanel01.SetActive(true);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                if (GameObject.Find("RankingContent").transform.childCount <= 1)
                    StartCoroutine(WebServerManager.RankingPanelCoroutine());
                break;
            case PlayableButtonInfo.Info.SETTING_01:
                settingPanel01.SetActive(true);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                GameObject player_1 = GameObject.FindWithTag("Player");
                Destroy(player_1);
                
                break;
            case PlayableButtonInfo.Info.GAME_START_02:
                if (!playerController.GetIsReady())
                {
                    playerController.SetIsReady(true);
                    AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_GAZE);
                }
                break;
            case PlayableButtonInfo.Info.BACK_02:
                LeaveRoomAndLoadScene(1);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            case PlayableButtonInfo.Info.FIND_ROOM_02:
                tempPlayer = GameObject.FindWithTag("Player");
                tempPlayer.GetComponent<BoxCollider>().enabled = false;
                roomListPanel02.SetActive(true);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            case PlayableButtonInfo.Info.CONTINUE_04:
                // 다음 라운드 게임 시작 
                machineUpgradePanel04.SetActive(false);
                engineDesPanel04.SetActive(false);
                storageDesPanel04.SetActive(false);
                if (!playerController.GetIsReady())
                {
                    playerController.SetIsReady(true);
                }
                break;
            case PlayableButtonInfo.Info.ENGINE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    ActiveAndDeActive(engineDesPanel04, machineUpgradePanel04, storageDesPanel04);
                    object[] data = new object[] { (int)_info,true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                break;
            case PlayableButtonInfo.Info.STORAGE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    ActiveAndDeActive(storageDesPanel04, machineUpgradePanel04, engineDesPanel04);
                    object[] data = new object[] { (int)_info, true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                break;
            case PlayableButtonInfo.Info.MACHINE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    ActiveAndDeActive(machineUpgradePanel04, storageDesPanel04, engineDesPanel04);
                    object[] data = new object[] { (int)_info, true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                break;
            case PlayableButtonInfo.Info.PRODUCTIONMACHINE_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                    object[] data = new object[] { (int)_info, true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                    AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                }
                break;
            case PlayableButtonInfo.Info.DYNAMITEMACHINE_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                    object[] data = new object[] { (int)_info, true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                    AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                }
                break;
            case PlayableButtonInfo.Info.WATERTANK_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                    object[] data = new object[] { (int)_info, true };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                    AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                }
                break;

            case PlayableButtonInfo.Info.GAME_EXIT_04:
                // 로비로 돌아가기
                machineUpgradePanel04.SetActive(false);
                engineDesPanel04.SetActive(false);
                storageDesPanel04.SetActive(false);
                LeaveRoomAndLoadScene(2);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;

            case PlayableButtonInfo.Info.REPLAY_05:
                if (!playerController.GetIsReady())
                    playerController.SetIsReady(true);
                break;
            case PlayableButtonInfo.Info.BACKTOLOBBY_05:
                LeaveRoomAndLoadScene(2);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            case PlayableButtonInfo.Info.REPLAY_06:
                if (!playerController.GetIsReady())
                    playerController.SetIsReady(true);
                break;
            case PlayableButtonInfo.Info.BACKTOMAIN_06:
                LeaveRoomAndLoadScene(1);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            case PlayableButtonInfo.Info.BACKTOLOBBY_06:
                LeaveRoomAndLoadScene(2);
                AudioManager.Instnce().PlaySFX(GetComponent<AudioSource>(), SOUNDTYPE.UI_CLICK);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 플레이어가 해당 버튼에서 액션키버튼을 눌렀을때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    public void PlayAbleButton_OnHit(PlayableButtonInfo.Info _info)
    {

    }

    #region Scene04_PlayAbleButton_OnHit
    public void PlayAbleButton_OnHit(PlayableButtonInfo _info)
    {
        switch (_info.myInfo)
        {
            case PlayableButtonInfo.Info.ENGINE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.UpgradeEngine();
                    enginePriceText04.text = StateManager.Instance().engineUpgradePrice.ToString();
                    engineDesText04.text = $"엔진 현재 용량 \n {StateManager.Instance().engineCurrentVolume} / {StateManager.Instance().engineMaxVolume}";
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    object[] data = new object[] { (int)_info.myInfo, false };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.STORAGE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.UpgradeStorage();
                    storagePriceText04.text = StateManager.Instance().storageUpgradePrice.ToString();
                    storageDesText04.text = $"저장소 현재 용량 \n {StateManager.Instance().storageMaxVolume}";
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    object[] data = new object[] { (int)_info.myInfo, false };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.PRODUCTIONMACHINE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.ProductionMachine, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["ProductionMachine"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    int count = 0;
                    for (int i = 0; i < productionMachineList.Count; i++)
                    {
                        if (_info.gameObject == productionMachineList[i])
                        {
                            count = i;
                        }
                    }
                    object[] data = new object[] { (int)_info.myInfo, false,count };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.DYNAMITEMACHINE_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.DynamiteMachine, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["DynamiteMachine"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    int count = 0;
                    for (int i = 0; i < dynamiteMachineList.Count; i++)
                    {
                        if (_info.gameObject == dynamiteMachineList[i])
                        {
                            count = i;
                        }
                    }
                    object[] data = new object[] { (int)_info.myInfo, false,count };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.WATERTANK_UPGRADE_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.WaterTank, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["WaterTank"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    int count = 0;
                    for (int i = 0; i < waterTankList.Count; i++)
                    {
                        if (_info.gameObject == waterTankList[i])
                        {
                            count = i;
                        }
                    }
                    object[] data = new object[] { (int)_info.myInfo, false,count };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.PRODUCTIONMACHINE_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.ProductionMachine);
                    productionMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.ProductionMachine.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                    object[] data = new object[] { (int)_info.myInfo, false };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.DYNAMITEMACHINE_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.DynamiteMachine);
                    dynamiteMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.DynamiteMachine.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                    object[] data = new object[] { (int)_info.myInfo, false };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;
            case PlayableButtonInfo.Info.WATERTANK_BUY_04:
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.WaterTank);
                    waterTankBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.WaterTank.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                    object[] data = new object[] { (int)_info.myInfo, false };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UI_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                }
                

                break;

            default:
                break;
        }
    }
    #endregion

    /// <summary>
    /// 특정 오브젝트의 Active를 끄거나 켠다
    /// </summary>
    /// <param name="_activeGameObject">SetActive true를 해야될 객체</param>
    /// <param name="_deActiveGameObject">SetActive false를 해야될 객체</param>
    public void ActiveAndDeActive(GameObject _activeGameObject,GameObject _deActiveGameObject)
    {
        _activeGameObject.SetActive(true);
        _deActiveGameObject.SetActive(false);
    }

    /// <summary>
    /// 특정 오브젝트의 active를 켜고, 특정 오브젝트들의 active 을끈다
    /// </summary>
    /// <param name="_activeGameObject">SetActive true를 해야될 객체</param>
    /// <param name="_deActiveGameObjects">SetActive false를 해야될 객체들</param>
    public void ActiveAndDeActive(GameObject _activeGameObject, params GameObject[] _deActiveGameObjects)
    {
        _activeGameObject.SetActive(true);
        foreach (GameObject obj in _deActiveGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void SetText(Text _text,string _str)
    {
        _text.text = _str;
    }
    public void SetText(TextMeshPro _text, string _str)
    {
        _text.text = _str;
    }

    public void LoginButtonOnClick_01()
    {
        string user_id = loginPanel01.transform.Find("LoginTxt").Find("InputID").GetComponent<InputField>().text;
        string user_password = loginPanel01.transform.Find("LoginTxt").Find("InputPW").GetComponent<InputField>().text;
        StartCoroutine(WebServerManager.LoginCoroutine(user_id, user_password));
    }

    public void RankingPanelOff_01()
    {
        rankingPanel01.SetActive(false);

        GameObject player = Instantiate(Resources.Load<GameObject>("Player_1"));
        
        player.transform.position = new Vector3(25, 20, 0);
    }
    

    public void SettingPanelOff_01()
    {
        settingPanel01.SetActive(false);
        GameObject player = Instantiate(Resources.Load<GameObject>("Player_1"));
        PlayerPrefs.SetFloat("bgm_Volume", AudioManager.Instnce().bgmPlayer.volume);
        for (int i = 0; i < AudioManager.Instnce().sfxPlayer.Length;i++)
        PlayerPrefs.SetFloat("sfx_Volume", AudioManager.Instnce().sfxPlayer[i].volume);

        player.transform.position = new Vector3(25, 20, 0);
    }

    public void KeySetRight()
    {
        PlayerPrefs.SetInt(keySet, (int)KeyCode.LeftControl);
        SetText(keySettingText01, ((KeyCode)PlayerPrefs.GetInt(keySet)).ToString());
        
        KeyCodeInfo.myActionKeyCode = (KeyCode)PlayerPrefs.GetInt(keySet);
        Debug.Log(KeyCodeInfo.myActionKeyCode);

    }

    public void KeySetLeft()
    {
        KeyCodeInfo.myActionKeyCode = KeyCode.Space;
        PlayerPrefs.SetInt(keySet, (int)KeyCode.Space);
        SetText(keySettingText01, KeyCodeInfo.myActionKeyCode.ToString());
    }

    
    /// <summary>
    /// 각씬 별로 panel및 playablebutton 초기화
    /// </summary>
    public void Init()
    {
        //playerController = GetComponent<PlayerController>();

        #region Scene00
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ground = GameObject.Find("Ground");
            canvas = GameObject.Find("Canvas");
            loadPanel = canvas.transform.GetChild(0).gameObject;
            clip = GameObject.Find("LoadingRail").GetComponent<PlayableDirector>();
            OnSpaceKeyPress += CheckSpaceKeyUp;
            if (OnSpaceKeyPress != null)
            {
                OnSpaceKeyPress();
            }
        }
        else
        {
            OnSpaceKeyPress -= CheckSpaceKeyUp;
        }
        #endregion

        #region Scene01
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ground = GameObject.Find("Ground");
            canvas = GameObject.Find("Canvas");
            playableButton_GameStart01 = ground.transform.Find("GameStart").gameObject;
            playableButton_Ranking01 = ground.transform.Find("Ranking").gameObject;
            playableButton_Setting01 = ground.transform.Find("Setting").gameObject;
            playableButton_GameExit01 = ground.transform.Find("Quit").gameObject;

            playableButton_GameStart01.AddComponent<PlayableButtonInfo>();
            playableButton_Ranking01.AddComponent<PlayableButtonInfo>();
            playableButton_Setting01.AddComponent<PlayableButtonInfo>();
            playableButton_GameExit01.AddComponent<PlayableButtonInfo>();

            playableButton_GameStart01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_START_01;
            playableButton_Ranking01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.RANKING_01;
            playableButton_Setting01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.SETTING_01;
            playableButton_GameExit01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_EXIT_01;

            loginPanel01 = canvas.transform.Find("LoginPanel").gameObject;
            loginPanel01.transform.Find("LoginBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            loginPanel01.transform.Find("LoginBtn").GetComponent<Button>().onClick.AddListener(LoginButtonOnClick_01);
            settingPanel01 = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel01 = canvas.transform.Find("LoginFailPanel").gameObject;
            rankingPanel01 = canvas.transform.Find("RankingPanel").gameObject;
            rankingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.RemoveAllListeners();
            rankingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.AddListener(RankingPanelOff_01);
            settingPanel01.transform.Find("Setting").transform.Find("XButton").GetComponent<Button>().onClick.RemoveAllListeners();
            settingPanel01.transform.Find("Setting").transform.Find("XButton").GetComponent<Button>().onClick.AddListener(SettingPanelOff_01);
            settingPanel01.transform.Find("Setting").transform.Find("KeySettingTxt").transform.Find("KeySet").transform.Find("KeySetLeft").GetComponent<Button>().onClick.RemoveAllListeners();
            settingPanel01.transform.Find("Setting").transform.Find("KeySettingTxt").transform.Find("KeySet").transform.Find("KeySetLeft").GetComponent<Button>().onClick.AddListener(KeySetLeft);
            settingPanel01.transform.Find("Setting").transform.Find("KeySettingTxt").transform.Find("KeySet").transform.Find("KeySetRight").GetComponent<Button>().onClick.RemoveAllListeners();
            settingPanel01.transform.Find("Setting").transform.Find("KeySettingTxt").transform.Find("KeySet").transform.Find("KeySetRight").GetComponent<Button>().onClick.AddListener(KeySetRight);
            keySettingText01 = settingPanel01.transform.Find("Setting").transform.Find("KeySettingTxt").transform.Find("KeySet").transform.Find("KeySetTxt").GetComponent<Text>();
            rankingBarPrefab = Resources.Load<GameObject>("RankingBar");
            AudioManager.Instnce().bgmSlider = settingPanel01.transform.Find("Setting").transform.Find("SoundTxt").transform.Find("SoundSlider").GetComponent<Slider>();
            AudioManager.Instnce().bgmSlider.onValueChanged.AddListener(delegate { AudioManager.Instnce().SaveBGMVolume(); });
            AudioManager.Instnce().bgmSlider.onValueChanged.AddListener(delegate { AudioManager.Instnce().ChangeBGMVolume(); });
            AudioManager.Instnce().bgmSlider.value = PlayerPrefs.GetFloat("bgm_Volume");
            AudioManager.Instnce().sfxSlider = settingPanel01.transform.Find("Setting").transform.Find("SFXSoundTxt").transform.Find("SoundSlider").GetComponent<Slider>();
            AudioManager.Instnce().sfxSlider.onValueChanged.AddListener(delegate { AudioManager.Instnce().SaveSFXVolume(); });
            AudioManager.Instnce().sfxSlider.onValueChanged.AddListener(delegate { AudioManager.Instnce().ChangeSFXVolume(); });
            AudioManager.Instnce().sfxSlider.value = PlayerPrefs.GetFloat("sfx_Volume"); 
            PlayerPrefs.GetInt(keySet);
            
            keySettingText01.text = KeyCodeInfo.myActionKeyCode.ToString();

        }
        #endregion
        #region Scene02
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            
            ground = GameObject.Find("Ground");
            canvas = GameObject.Find("Canvas");
            playableButton_GameStart02 = ground.transform.Find("GameStart").gameObject;
            playableButton_Back02 = ground.transform.Find("BacktoMain").gameObject;
            playableButton_FindRoom02 = ground.transform.Find("EnterRoom").gameObject;

            playableButton_GameStart02.AddComponent<PlayableButtonInfo>();
            playableButton_Back02.AddComponent<PlayableButtonInfo>();
            playableButton_FindRoom02.AddComponent<PlayableButtonInfo>();

            playableButton_GameStart02.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_START_02;
            playableButton_Back02.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.BACK_02;
            playableButton_FindRoom02.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.FIND_ROOM_02;

            loginFailPanel02 = canvas.transform.Find("LoginFailPanel").gameObject;
            roomListPanel02 = canvas.transform.Find("RoomListPanel").gameObject;
            roomListPanel02.transform.Find("RoomListBackGround").transform.Find("XButton").GetComponent<Button>().onClick.RemoveAllListeners();
            roomListPanel02.transform.Find("RoomListBackGround").transform.Find("XButton").GetComponent<Button>().onClick.AddListener(() => OnClickCloseBtn(closeBtnType.ROOMLIST01_CLOSE));
            roomIdText02 = ground.transform.Find("RoomNumber").transform.Find("RoomIDTxt").GetComponent<TextMeshPro>();
            
        }
        #endregion
        #region Scene03
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            canvas = GameObject.Find("Canvas");
            rDVindex03 = canvas.transform.Find("RDVIndex").gameObject;
            speedIndex03 = canvas.transform.Find("SpeedIndex").gameObject;
            questIndex03 = canvas.transform.Find("QuestIndex").gameObject;
            chat = canvas.transform.Find("Chat").gameObject;

            distance03 = rDVindex03.transform.Find("DistanceTxt").transform.Find("Distance").GetComponent<Text>();
            volt03 = rDVindex03.transform.Find("VoltTxt").transform.Find("Volt").GetComponent<Text>();
            SetText(rDVindex03.transform.Find("RoomTxt").Find("RoomID").GetComponent<Text>(), PhotonNetwork.CurrentRoom.Name);
            string _round = string.Format("{00}", GameManager.Instance().GetRound());
            SetText(speedIndex03.transform.Find("SpeedTxt").transform.Find("Speed").GetComponent<Text>(), _round);
        }


        #endregion
        #region Scene04
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            ground = GameObject.Find("Ground");
            canvas = GameObject.Find("Canvas");
            upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();

            playableButton_CONTINUE_04 = ground.transform.Find("Continue").gameObject;
            playableButton_GAME_EXIT_04 = ground.transform.Find("BacktoLobby").gameObject;
            playableButton_ENGINE_UPGRADE_04 = ground.transform.Find("EngineUpgrade").gameObject;
            playableButton_STORAGE_UPGRADE_04 = ground.transform.Find("StorageUpgrade").gameObject;
            playableButton_MACHINE_UPGRADE_04 = ground.transform.Find("MachineUpgrade").gameObject;
            playableButton_PRODUCTIONMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(1).gameObject;
            playableButton_DYNAMITEMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(2).gameObject;
            playableButton_WATERTANK_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(3).gameObject;

            machineUpgradePanel04 = ground.transform.Find("MachineUpgradePanel").gameObject;
            engineDesPanel04 = playableButton_ENGINE_UPGRADE_04.transform.GetChild(3).gameObject;
            storageDesPanel04 = playableButton_STORAGE_UPGRADE_04.transform.GetChild(3).gameObject;




            playableButton_CONTINUE_04.AddComponent<PlayableButtonInfo>();
            playableButton_GAME_EXIT_04.AddComponent<PlayableButtonInfo>();
            playableButton_ENGINE_UPGRADE_04.AddComponent<PlayableButtonInfo>();
            playableButton_STORAGE_UPGRADE_04.AddComponent<PlayableButtonInfo>();
            playableButton_MACHINE_UPGRADE_04.AddComponent<PlayableButtonInfo>();
            playableButton_PRODUCTIONMACHINE_BUY_04.AddComponent<PlayableButtonInfo>();
            playableButton_DYNAMITEMACHINE_BUY_04.AddComponent<PlayableButtonInfo>();
            playableButton_WATERTANK_BUY_04.AddComponent<PlayableButtonInfo>();

            playableButton_CONTINUE_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.CONTINUE_04;
            playableButton_GAME_EXIT_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_EXIT_04;
            playableButton_ENGINE_UPGRADE_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.ENGINE_UPGRADE_04;
            playableButton_STORAGE_UPGRADE_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.STORAGE_UPGRADE_04;
            playableButton_MACHINE_UPGRADE_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.MACHINE_UPGRADE_04;
            playableButton_PRODUCTIONMACHINE_BUY_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.PRODUCTIONMACHINE_BUY_04;
            playableButton_DYNAMITEMACHINE_BUY_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.DYNAMITEMACHINE_BUY_04;
            playableButton_WATERTANK_BUY_04.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.WATERTANK_BUY_04;

            voltNumText04 = canvas.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
            enginePriceText04 = playableButton_ENGINE_UPGRADE_04.transform.GetChild(2).GetComponent<TextMeshPro>();
            engineDesText04 = engineDesPanel04.GetComponentInChildren<TextMeshPro>();
            storagePriceText04 = playableButton_STORAGE_UPGRADE_04.transform.GetChild(2).GetComponent<TextMeshPro>();
            storageDesText04 = storageDesPanel04.GetComponentInChildren<TextMeshPro>();
            productionMachineBuyPriceText04 = playableButton_PRODUCTIONMACHINE_BUY_04.transform.Find("PriceTxt").GetComponent<TextMeshPro>();
            dynamiteMachineBuyPriceText04 = playableButton_DYNAMITEMACHINE_BUY_04.transform.Find("PriceTxt").GetComponent<TextMeshPro>();
            waterTankBuyPriceText04 = playableButton_WATERTANK_BUY_04.transform.Find("PriceTxt").GetComponent<TextMeshPro>();

            for (int i = 0; i < machineUpgradePanel04.transform.GetChild(0).transform.childCount; i++)
            {
                pos[i] = machineUpgradePanel04.transform.GetChild(0).transform.GetChild(i);
            }

            upgradeManager.ShowUpgradeMachine(pos);

            storageDesText04.text = $"저장소 현재 용량\n {StateManager.Instance().storageMaxVolume}";
            engineDesText04.text = $"엔진 현재 용량 \n {StateManager.Instance().engineCurrentVolume} / {StateManager.Instance().engineMaxVolume}";

            RDVIndex04 = canvas.transform.Find("RDVindex").gameObject;
            speedIndex04 = canvas.transform.Find("SpeedIndex").gameObject;

            SetText(RDVIndex04.transform.Find("RoomTxt").Find("RoomID").GetComponent<Text>(), PhotonNetwork.CurrentRoom.Name);
            string _round = string.Format("{00}", GameManager.Instance().GetRound());
            SetText(speedIndex04.transform.Find("SpeedTxt").transform.Find("Speed").GetComponent<Text>(), _round);
            SetText(RDVIndex04.transform.Find("DistanceTxt").transform.Find("Distance").GetComponent<Text>(), ((int)GameManager.Instance().GetMeter())+"M");

            storagePriceText04.text = StateManager.Instance().storageUpgradePrice.ToString();
            enginePriceText04.text = StateManager.Instance().engineUpgradePrice.ToString();
            productionMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.ProductionMachine.ToString()].ToString();
            waterTankBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.WaterTank.ToString()].ToString();
            dynamiteMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.DynamiteMachine.ToString()].ToString();

        }
        #endregion

        #region Scene05
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            ground = GameObject.Find("Ground");
            playableButton_ReStart05 = ground.transform.Find("RePlay").gameObject;
            playableButton_Back05 = ground.transform.Find("BacktoLobby").gameObject;
            roundText05 = ground.transform.Find("RoundRecord").transform.Find("RoundRecordTxt").GetComponent<TextMeshPro>();
            gameOverTimeText05 = ground.transform.Find("TimeRecord").transform.Find("TimeRecordTxt").GetComponent<TextMeshPro>();

            playableButton_ReStart05.AddComponent<PlayableButtonInfo>();
            playableButton_Back05.AddComponent<PlayableButtonInfo>();

            playableButton_ReStart05.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.REPLAY_05;
            playableButton_Back05.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.BACKTOLOBBY_05;
           
            SetText(roundText05, GameManager.Instance().GetRound().ToString() + "Round");
            //SetText(gameOverTimeText05, TimeManager.Instance().GetCurTime().ToString());
            int time = (int)TimeManager.Instance().PrevTime;
            string formattedTime = string.Format("{0:00}:{1:00}", time / 60, time % 60);
            SetText(gameOverTimeText05, formattedTime);
            Debug.Log("게임오버 시간 :::"+ ((int)TimeManager.Instance().PrevTime).ToString());
            if (GameManager.Instance() != null)
            {
                playerController = PhotonNetwork.Instantiate("Player", new Vector3(25, 20, 0), Quaternion.identity).GetComponent<PlayerController>();
            }
            if (GameObject.Find("TeamManager") != null)
            {
                TeamManager teamManager = GameObject.Find("TeamManager").GetComponent<TeamManager>();
                teamManager.SetNeedReadyUserCount(true);
            }
            
            GameManager.Instance().SetRound(1);
        }
        #endregion

        #region Scene06
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            ground = GameObject.Find("Ground");

            playableButton_ReStart06 = ground.transform.Find("RePlay").gameObject;
            playableButton_BackToLobby06 = ground.transform.Find("BacktoLobby").gameObject;
            playableButton_BackToMain06 = ground.transform.Find("Main").gameObject;

            playableButton_ReStart06.AddComponent<PlayableButtonInfo>();
            playableButton_BackToLobby06.AddComponent<PlayableButtonInfo>();
            playableButton_BackToMain06.AddComponent<PlayableButtonInfo>();

            playableButton_ReStart06.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.REPLAY_06;
            playableButton_BackToMain06.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.BACKTOMAIN_06;
            playableButton_BackToLobby06.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.BACKTOLOBBY_06;

            clearTimeText06 = ground.transform.Find("FinalRecord").transform.Find("FinalTimeTxt").GetComponent<TextMeshPro>();
            storageLvText06 = ground.transform.Find("FinalStorage").transform.Find("FinalStorageTxt").GetComponent<TextMeshPro>();
            dynamiteLvText06 = ground.transform.Find("FinalDynamite").transform.Find("FinalDynamiteTxt").GetComponent<TextMeshPro>();
            productionLvText06 = ground.transform.Find("FinalProduction").transform.Find("FinalProductionTxt").GetComponent<TextMeshPro>();
            watertankLvText06 = ground.transform.Find("FinalWaterTank").transform.Find("FinalWaterTankTxt").GetComponent<TextMeshPro>();

            int time = (int)TimeManager.Instance().finalTime;
            string formattedTime = string.Format("{0:00}:{1:00}", time / 60, time % 60);

            //TODO : 0208_김주완: 같은 방에 있는 플레이어의 userId를 리스트로 저장
            if (PhotonNetwork.IsMasterClient)
            {
                List<string> playerNickNames = PhotonNetwork.PlayerList.Select(player => player.NickName).ToList();
                StartCoroutine(WebServerManager.InsertDataCoroutine(PhotonNetwork.CurrentRoom.Name, TimeManager.Instance().finalTime, TimeManager.Instance().roundClearTimeList[0], TimeManager.Instance().roundClearTimeList[1], TimeManager.Instance().roundClearTimeList[2], TimeManager.Instance().roundClearTimeList[3], TimeManager.Instance().roundClearTimeList[4], playerNickNames));
            }

            SetText(clearTimeText06, formattedTime);
            SetText(storageLvText06, StateManager.Instance().storageMaxVolume.ToString());
            SetText(dynamiteLvText06, StateManager.Instance().dynamiteMachines.Count.ToString());
            SetText(productionLvText06, StateManager.Instance().productionMachines.Count.ToString());
            SetText(watertankLvText06, StateManager.Instance().waterTanks.Count.ToString());


            TimeManager.Instance().roundClearTimeList.Clear();
            TimeManager.Instance().finalTime = 0;
            GameManager.Instance().SetRound(1);
            GameManager.Instance().GameExit();
            PhotonNetwork.Instantiate("Player", new Vector3(25, 20, 0), Quaternion.identity);

        }
        #endregion
    }

    [PunRPC]
    void SetText_Others(string _str)
    {
        clearTimeText06.text = _str;
    }

    #region Photon
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.UI_INFO && !PhotonNetwork.IsMasterClient)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            PlayableButtonInfo.Info _uiInfo = (PlayableButtonInfo.Info)((int)receivedData[0]);
            bool isOnStay = (bool)receivedData[1];
            if (isOnStay)
            {
                if (_uiInfo.Equals(PlayableButtonInfo.Info.ENGINE_UPGRADE_04))
                {
                    ActiveAndDeActive(engineDesPanel04, machineUpgradePanel04, storageDesPanel04);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.STORAGE_UPGRADE_04))
                {
                    ActiveAndDeActive(storageDesPanel04, machineUpgradePanel04, engineDesPanel04);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.MACHINE_UPGRADE_04))
                {
                    ActiveAndDeActive(machineUpgradePanel04, storageDesPanel04, engineDesPanel04);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.PRODUCTIONMACHINE_BUY_04))
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.DYNAMITEMACHINE_BUY_04))
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.WATERTANK_BUY_04))
                {
                    machineUpgradePanel04.SetActive(false);
                    engineDesPanel04.SetActive(false);
                    storageDesPanel04.SetActive(false);
                }
            }
            else
            {
                if (_uiInfo.Equals(PlayableButtonInfo.Info.ENGINE_UPGRADE_04)) 
                {
                    upgradeManager.UpgradeEngine();
                    enginePriceText04.text = StateManager.Instance().engineUpgradePrice.ToString();
                    engineDesText04.text = $"엔진 현재 용량 \n {StateManager.Instance().engineCurrentVolume} / {StateManager.Instance().engineMaxVolume}";
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.STORAGE_UPGRADE_04)) 
                {
                    upgradeManager.UpgradeStorage();
                    storagePriceText04.text = StateManager.Instance().storageUpgradePrice.ToString();
                    storageDesText04.text = $"저장소 현재 용량\n {StateManager.Instance().storageMaxVolume}";
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.PRODUCTIONMACHINE_UPGRADE_04)) 
                {
                    PlayableButtonInfo _info = productionMachineList[(int)receivedData[2]].GetComponent<PlayableButtonInfo>();
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.ProductionMachine, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["ProductionMachine"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.DYNAMITEMACHINE_UPGRADE_04)) 
                {
                    PlayableButtonInfo _info = dynamiteMachineList[(int)receivedData[2]].GetComponent<PlayableButtonInfo>();
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.DynamiteMachine, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["DynamiteMachine"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.WATERTANK_UPGRADE_04)) 
                {
                    PlayableButtonInfo _info = waterTankList[(int)receivedData[2]].GetComponent<PlayableButtonInfo>();
                    upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.WaterTank, _info.machineUpgradeIDX);
                    _info.transform.GetChild(1).GetComponent<TextMeshPro>().text = StateManager.Instance().factoryPrice["WaterTank"][_info.machineUpgradeIDX].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.PRODUCTIONMACHINE_BUY_04)) 
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.ProductionMachine);
                    productionMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.ProductionMachine.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.DYNAMITEMACHINE_BUY_04)) 
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.DynamiteMachine);
                    dynamiteMachineBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.DynamiteMachine.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                }
                if (_uiInfo.Equals(PlayableButtonInfo.Info.WATERTANK_BUY_04)) 
                {
                    upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.WaterTank);
                    waterTankBuyPriceText04.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.WaterTank.ToString()].ToString();
                    voltNumText04.text = StateManager.Instance().voltNum.ToString();
                    upgradeManager.ClearUpgradeMachine(pos);
                    upgradeManager.ShowUpgradeMachine(pos);
                }
            }
        }
        
    }

    

    #endregion

    #region OtherFunc
    public GameObject CreateBar()
    {
        return Instantiate(Resources.Load<GameObject>(rankingBarPrefab.name));
    }

    
    public void LeaveRoomAndLoadScene(int sceneIndex)
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(LoadSceneAfterLeftRoom(sceneIndex));
    }

    private IEnumerator LoadSceneAfterLeftRoom(int sceneIndex)
    {
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        GameManager.Instance().GameExit();
        SceneManager.LoadScene(sceneIndex);
    }

    void OnClickCloseBtn(closeBtnType type)
    {
        if (type.Equals(closeBtnType.ROOMLIST01_CLOSE))
        {
            tempPlayer.transform.position = new Vector3(25, 20, 0);
            tempPlayer.GetComponent<BoxCollider>().enabled = true;
            tempPlayer = null;
            roomListPanel02.SetActive(false);
        }
    }

    #endregion

    #region Event
    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        OnSpaceKeyPress -= CheckSpaceKeyUp;
    }


    void CheckSpaceKeyUp()
    {
        StartCoroutine(CheckSpaceKeyUp_Coroutine());
    }

    IEnumerator CheckSpaceKeyUp_Coroutine()
    {
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        Debug.Log("실행됨");
        StartLoading();
    }

    #endregion
}
