using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.MACHINE_UPGRADE_04:

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.GAME_EXIT_04:
                // 로비로 돌아가기

                break;
            default:
                break;
        }
    }
    #endregion

    #region Scene04_PlayAbleButton_OnHit
    public void PlayAbleButton_OnHit(PlayableButtonInfo_LeeYuJoung.Info _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo_LeeYuJoung.Info.ENGINE_UPGRADE_04:

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.STORAGE_UPGRADE_04:

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.PRODUCTIONMACHINE_BUY_04:

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.DYNAMITEMACHINE_BUY_04:

                break;
            case PlayableButtonInfo_LeeYuJoung.Info.WATERTANK_BUY_04:

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
        ground = GameObject.Find("Ground");
        canvas = GameObject.Find("Canvas");
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
            playableButton_CONTINUE_04 = ground.transform.Find("Continue").gameObject;
            playableButton_GAME_EXIT_04 = ground.transform.Find("BacktoLobby").gameObject;
            playableButton_ENGINE_UPGRADE_04 = ground.transform.Find("EngineUpgrade").transform.GetChild(1).gameObject;
            playableButton_STORAGE_UPGRADE_04 = ground.transform.Find("StorageUpgrade").transform.GetChild(1).gameObject;
            playableButton_MACHINE_UPGRADE_04 = ground.transform.Find("MachineUpgrade").gameObject;
            playableButton_PRODUCTIONMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(1).gameObject;
            playableButton_DYNAMITEMACHINE_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(2).gameObject;
            playableButton_WATERTANK_BUY_04 = ground.transform.Find("MachineBuy").transform.GetChild(3).gameObject;

            playableButton_CONTINUE_04.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo = PlayableButtonInfo_LeeYuJoung.Info.CONTINUE_04;

        }
        #endregion
    }
}
