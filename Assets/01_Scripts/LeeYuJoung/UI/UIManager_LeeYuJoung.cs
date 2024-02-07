using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LeeYuJoung;
using UnityEngine.Playables;

public class UIManager_LeeYuJoung : MonoBehaviour
{
    #region Instance
    private static UIManager_LeeYuJoung instance;
    public static UIManager_LeeYuJoung Instance()
    {
        return instance;
    }
    #endregion
    public PlayerController playerController;


    #region Scene00
    public GameObject loadPanel;
    public PlayableDirector clip;
    #endregion

    #region Scene01
    [Header("PlayableButtons")]
    public GameObject ground;
    public GameObject playableButton_GameStart01;
    public GameObject playableButton_Ranking01;
    public GameObject playableButton_GameExit01;
    public GameObject playableButton_Setting01;
    public GameObject canvas;
    [Header("Panel")]
    public GameObject loginPanel;
    public GameObject settingPanel;
    public GameObject loginFailPanel;
    #endregion

    #region Scene02

    #endregion

    #region Scene04
    public UpgradeManager upgradeManager;
    public Transform[] pos = new Transform[9];

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
    public GameObject machineUpgradePanel;
    public GameObject engineDesPanel;
    public GameObject storageDesPanel;
    [Header("Text")]
    public Text voltNumText;
    public TextMesh enginePriceText;
    public TextMesh engineDesText;
    public TextMesh storagePriceText;
    public TextMesh storageDesText;
    public TextMesh productionMachineBuyPriceText;
    public TextMesh dynamiteMachineBuyPriceText;
    public TextMesh waterTankBuyPriceText;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartLoading();
        }
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
                ActiveAndDeActive(loginPanel, ground);
                break;
            case PlayableButtonInfo.Info.GAME_EXIT_01:
                break;
            case PlayableButtonInfo.Info.RANKING_01:
                break;
            case PlayableButtonInfo.Info.SETTING_01:
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

    #region Scene04_PlayAbleButton_OnStay
    public void PlayAbleButton_OnStay(PlayableButtonInfo_LeeYuJoung.Info _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo_LeeYuJoung.Info.CONTINUE_04:
                // 다음 라운드 게임 시작 
                machineUpgradePanel.SetActive(false);
                engineDesPanel.SetActive(false);
                storageDesPanel.SetActive(false);

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.ENGINE_UPGRADE_04:
                ActiveAndDeActive(engineDesPanel, new GameObject[] { machineUpgradePanel, storageDesPanel });

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.STORAGE_UPGRADE_04:
                ActiveAndDeActive(storageDesPanel, new GameObject[] { machineUpgradePanel, engineDesPanel });

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.MACHINE_UPGRADE_04:
                ActiveAndDeActive(machineUpgradePanel, new GameObject[] { storageDesPanel, engineDesPanel });

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.PRODUCTIONMACHINE_BUY_04:
                machineUpgradePanel.SetActive(false);
                engineDesPanel.SetActive(false);
                storageDesPanel.SetActive(false);

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.DYNAMITEMACHINE_BUY_04:
                machineUpgradePanel.SetActive(false);
                engineDesPanel.SetActive(false);
                storageDesPanel.SetActive(false);

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.WATERTANK_BUY_04:
                machineUpgradePanel.SetActive(false);
                engineDesPanel.SetActive(false);
                storageDesPanel.SetActive(false);

                break;

            case PlayableButtonInfo_LeeYuJoung.Info.GAME_EXIT_04:
                // 로비로 돌아가기
                machineUpgradePanel.SetActive(false);
                engineDesPanel.SetActive(false);
                storageDesPanel.SetActive(false);

                break;
            default:
                break;
        }
    }
    #endregion

    #region Scene04_PlayAbleButton_OnHit
    public void PlayAbleButton_OnHit(PlayableButtonInfo_LeeYuJoung info)
    {
        switch (info.myInfo)
        {
            case PlayableButtonInfo_LeeYuJoung.Info.ENGINE_UPGRADE_04:
                upgradeManager.UpgradeEngine();
                enginePriceText.text = StateManager.Instance().engineUpgradePrice.ToString();
                engineDesText.text = $"엔진 현재 용량 \n {StateManager.Instance().engineCurrentVolume} / {StateManager.Instance().engineMaxVolume}";
                voltNumText.text = StateManager.Instance().voltNum.ToString();

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.STORAGE_UPGRADE_04:
                upgradeManager.UpgradeStorage();
                storagePriceText.text = StateManager.Instance().storageUpgradePrice.ToString();
                storageDesText.text = $"저장소 현재 용량 \n {StateManager.Instance().storageMaxVolume}";
                voltNumText.text = StateManager.Instance().voltNum.ToString();

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.PRODUCTIONMACHINE_UPGRADE_04:
                upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.ProductionMachine, info.machineUpgradeIDX);
                info.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["ProductionMachine"][info.machineUpgradeIDX].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.DYNAMITEMACHINE_UPGRADE_04:
                upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.DynamiteMachine, info.machineUpgradeIDX);
                info.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["DynamiteMachine"][info.machineUpgradeIDX].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.WATERTANK_UPGRADE_04:
                upgradeManager.UpgradeMachine(FactoryManager.FACTORYTYPE.WaterTank, info.machineUpgradeIDX);
                info.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["WaterTank"][info.machineUpgradeIDX].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.PRODUCTIONMACHINE_BUY_04:
                upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.ProductionMachine);
                productionMachineBuyPriceText.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.ProductionMachine.ToString()].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();
                upgradeManager.ClearUpgradeMachine(pos);
                upgradeManager.ShowUpgradeMachine(pos);

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.DYNAMITEMACHINE_BUY_04:
                upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.DynamiteMachine);
                dynamiteMachineBuyPriceText.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.DynamiteMachine.ToString()].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();
                upgradeManager.ClearUpgradeMachine(pos);
                upgradeManager.ShowUpgradeMachine(pos);

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.WATERTANK_BUY_04:
                upgradeManager.BuyMachine(FactoryManager.FACTORYTYPE.WaterTank);
                waterTankBuyPriceText.text = StateManager.Instance().machineAddPrice[FactoryManager.FACTORYTYPE.WaterTank.ToString()].ToString();
                voltNumText.text = StateManager.Instance().voltNum.ToString();
                upgradeManager.ClearUpgradeMachine(pos);
                upgradeManager.ShowUpgradeMachine(pos);

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

    public void LoginButtonOnClick_01()
    {
        string user_id = loginPanel.transform.Find("InputID").GetComponent<InputField>().text;
        string user_password = loginPanel.transform.Find("InputPW").GetComponent<InputField>().text;
        StartCoroutine(WebServerManager.LoginCoroutine(user_id, user_password));
    }

    /// <summary>
    /// 각씬 별로 panel및 playablebutton 초기화
    /// </summary>
    public void Init()
    {
        playerController = GetComponent<PlayerController>();
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ground = GameObject.Find("Ground");
        canvas = GameObject.Find("Canvas");

        #region Scene00
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            loadPanel = canvas.transform.GetChild(0).gameObject;
            clip = GameObject.Find("LoadingRail").GetComponent<PlayableDirector>();
        }
        #endregion

        #region Scene01
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playableButton_GameStart01 = ground.transform.Find("GameStart").gameObject;
            playableButton_Ranking01 = ground.transform.Find("Ranking").gameObject;
            playableButton_Setting01 = ground.transform.Find("Setting").gameObject;
            playableButton_GameExit01 = ground.transform.Find("Quit").gameObject;

            playableButton_GameStart01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_START_01;
            playableButton_Ranking01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.RANKING_01;
            playableButton_Setting01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.SETTING_01;
            playableButton_GameExit01.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_EXIT_01;

            loginPanel = canvas.transform.Find("LoginPanel").gameObject;
            settingPanel = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel = canvas.transform.Find("LoginFailPanel").gameObject;
        }
        #endregion

        #region Scene04
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();

            playableButton_CONTINUE_04 = ground.transform.Find("Continue").gameObject;
            playableButton_GAME_EXIT_04 = ground.transform.Find("BacktoLobby").gameObject;
            playableButton_ENGINE_UPGRADE_04 = ground.transform.Find("EngineUpgrade").gameObject;
            playableButton_STORAGE_UPGRADE_04 = ground.transform.Find("StorageUpgrade").gameObject;
            playableButton_MACHINE_UPGRADE_04 = ground.transform.Find("MachineUpgrade").gameObject;
            playableButton_PRODUCTIONMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(1).gameObject;
            playableButton_DYNAMITEMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(2).gameObject;
            playableButton_WATERTANK_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(3).gameObject;

            machineUpgradePanel = ground.transform.Find("MachineUpgradePanel").gameObject;
            engineDesPanel = playableButton_ENGINE_UPGRADE_04.transform.GetChild(3).gameObject;
            storageDesPanel = playableButton_STORAGE_UPGRADE_04.transform.GetChild(3).gameObject;

            playableButton_CONTINUE_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.CONTINUE_04;
            playableButton_GAME_EXIT_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.GAME_EXIT_04;
            playableButton_ENGINE_UPGRADE_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.ENGINE_UPGRADE_04;
            playableButton_STORAGE_UPGRADE_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.STORAGE_UPGRADE_04;
            playableButton_MACHINE_UPGRADE_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.MACHINE_UPGRADE_04;
            playableButton_PRODUCTIONMACHINE_BUY_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.PRODUCTIONMACHINE_BUY_04;
            playableButton_DYNAMITEMACHINE_BUY_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.DYNAMITEMACHINE_BUY_04;
            playableButton_WATERTANK_BUY_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.WATERTANK_BUY_04;

            voltNumText = canvas.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
            enginePriceText = playableButton_ENGINE_UPGRADE_04.transform.GetChild(2).GetComponent<TextMesh>();
            engineDesText = engineDesPanel.GetComponentInChildren<TextMesh>();  
            storagePriceText = playableButton_STORAGE_UPGRADE_04.transform.GetChild(2).GetComponent<TextMesh>();
            storageDesText = storageDesPanel.GetComponentInChildren<TextMesh>();
            productionMachineBuyPriceText = playableButton_PRODUCTIONMACHINE_BUY_04.transform.GetChild(1).GetComponent<TextMesh>();
            dynamiteMachineBuyPriceText = playableButton_DYNAMITEMACHINE_BUY_04.transform.GetChild(1).GetComponent<TextMesh>();
            waterTankBuyPriceText = playableButton_WATERTANK_BUY_04.transform.GetChild(1).GetComponent<TextMesh>();

            for (int i = 0; i < machineUpgradePanel.transform.GetChild(0).transform.childCount; i++)
            {
                pos[i] = machineUpgradePanel.transform.GetChild(0).transform.GetChild(i);
            }

            upgradeManager.ShowUpgradeMachine(pos);
        }
        #endregion
    }
}
