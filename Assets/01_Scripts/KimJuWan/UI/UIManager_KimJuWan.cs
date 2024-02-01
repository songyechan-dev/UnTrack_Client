using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GameManager;
using LeeYuJoung;

public class UIManager_KimJuWan : MonoBehaviour
{
    #region Instance
    private static UIManager_KimJuWan instance;
    public static UIManager_KimJuWan Instance()
    {
        return instance;
    }
    #endregion
    public PlayerController playerController;
    public GameObject canvas;
    public GameObject ground;
    public string keySet = "PlayerActionKeyCode";
    
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
    [Header("PlayableButtons")]
    public GameObject playableButton_GameStart02;
    public GameObject playableButton_Back02;
    public GameObject playableButton_FindRoom02;
    [Header("Panel")]
    public GameObject roomListPanel02;
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
        
        Debug.Log((KeyCode)PlayerPrefs.GetInt(keySet));
    }


    /// <summary>
    /// 플레이어가 해당 버튼에 머물때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    public void PlayAbleButton_OnStay(PlayableButtonInfo_KimJuWan.Info _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo_KimJuWan.Info.GAME_START_01:
                ActiveAndDeActive(loginPanel01, ground);
                break;
            case PlayableButtonInfo_KimJuWan.Info.GAME_EXIT_01:
                PlayerPrefs.Save();
                Application.Quit();
                break;
            case PlayableButtonInfo_KimJuWan.Info.RANKING_01:
                ActiveAndDeActive(rankingPanel01, ground);
                break;
            case PlayableButtonInfo_KimJuWan.Info.SETTING_01:
                ActiveAndDeActive(settingPanel01, ground);
                break;
            //case PlayableButtonInfo.Info.GAME_START_02:
            //    if (!playerController.GetIsReady())
            //    {
            //        playerController.SetIsReady(true);
            //    }
            //    break;
            //case PlayableButtonInfo.Info.BACK_02:
            //    break;
            //case PlayableButtonInfo.Info.FIND_ROOM_02:
            //    break;
            //default:
            //    break;
            case PlayableButtonInfo_KimJuWan.Info.REPLAY_05:
                if(!playerController.GetIsReady())
                    playerController.SetIsReady(true);
                break;
            case PlayableButtonInfo_KimJuWan.Info.BACKTOLOBBY_05:
                SceneManager.LoadScene("02_Lobby");
                break;
            case PlayableButtonInfo_KimJuWan.Info.REPLAY_06:
                if (!playerController.GetIsReady())
                    playerController.SetIsReady(true);
                break;
            case PlayableButtonInfo_KimJuWan.Info.BACKTOMAIN_06:
                SceneManager.LoadScene("01_Intro");
                
                break;
            case PlayableButtonInfo_KimJuWan.Info.BACKTOLOBBY_06:
                SceneManager.LoadScene("02_Lobby");
                //새로운 방 자동 생성
                break;
        }
    }

    /// <summary>
    /// 플레이어가 해당 버튼에서 액션키버튼을 눌렀을때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    public void PlayAbleButton_OnHit(PlayableButtonInfo_KimJuWan.Info _info)
    {

    }
    /// <summary>
    /// 특정 오브젝트의 Active를 끄거나 켠다
    /// </summary>
    /// <param name="_activeGameObject">SetActive true를 해야될 객체</param>
    /// <param name="_deActiveGameObject">SetActive false를 해야될 객체</param>
    public void ActiveAndDeActive(GameObject _activeGameObject, GameObject _deActiveGameObject)
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

    public void SetText(Text _text, string _str)
    {
        _text.text = _str;
    }
    public void SetText(TextMeshPro _text, string _str)
    {
        _text.text = _str;
    }

    public void LoginButtonOnClick_01()
    {
        string user_id = loginPanel01.transform.Find("InputID").GetComponent<InputField>().text;
        string user_password = loginPanel01.transform.Find("InputPW").GetComponent<InputField>().text;
        StartCoroutine(WebServerManager.LoginCoroutine(user_id, user_password));
    }

    public void RankingPanelOff_01()
    {
        ActiveAndDeActive(ground, rankingPanel01);
    }

    public void SettingPanelOff_01()
    {
        ActiveAndDeActive(ground, settingPanel01);
    }
    
    public void KeySetRight()
    {
                  
            keySettingText01.text = ((KeyCode)PlayerPrefs.GetInt(keySet)).ToString();
            KeyCodeInfo.myActionKeyCode = (KeyCode)PlayerPrefs.GetInt(keySet);
        Debug.Log(KeyCodeInfo.myActionKeyCode);
            
    }

    public void KeySetLeft()
    {
        KeyCodeInfo.myActionKeyCode = KeyCode.Space;
        keySettingText01.text = KeyCodeInfo.myActionKeyCode.ToString();
    }
    
    
    /// <summary>
    /// 각씬 별로 panel및 playablebutton 초기화
    /// </summary>
    public void Init()
    {
        //playerController = GetComponent<PlayerController>();

        #region Scene01
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ground = GameObject.Find("Ground");
            canvas = GameObject.Find("Canvas");
            playableButton_GameStart01 = ground.transform.Find("GameStart").gameObject;
            playableButton_Ranking01 = ground.transform.Find("Ranking").gameObject;
            playableButton_Setting01 = ground.transform.Find("Setting").gameObject;
            playableButton_GameExit01 = ground.transform.Find("Quit").gameObject;

            playableButton_GameStart01.AddComponent<PlayableButtonInfo_KimJuWan>();
            playableButton_Ranking01.AddComponent<PlayableButtonInfo_KimJuWan>();
            playableButton_Setting01.AddComponent<PlayableButtonInfo_KimJuWan>();
            playableButton_GameExit01.AddComponent<PlayableButtonInfo_KimJuWan>();

            playableButton_GameStart01.GetComponent<PlayableButtonInfo_KimJuWan>().myInfo = PlayableButtonInfo_KimJuWan.Info.GAME_START_01;
            playableButton_Ranking01.GetComponent<PlayableButtonInfo_KimJuWan>().myInfo = PlayableButtonInfo_KimJuWan.Info.RANKING_01;
            playableButton_Setting01.GetComponent<PlayableButtonInfo_KimJuWan>().myInfo = PlayableButtonInfo_KimJuWan.Info.SETTING_01;
            playableButton_GameExit01.GetComponent<PlayableButtonInfo_KimJuWan>().myInfo = PlayableButtonInfo_KimJuWan.Info.GAME_EXIT_01;

            loginPanel01 = canvas.transform.Find("LoginPanel").gameObject;
            loginPanel01.transform.Find("LoginBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            loginPanel01.transform.Find("LoginBtn").GetComponent<Button>().onClick.AddListener(LoginButtonOnClick_01);
            settingPanel01 = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel01 = canvas.transform.Find("LoginFailPanel").gameObject;
            rankingPanel01 = canvas.transform.Find("RankingPanel").gameObject;
            rankingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.RemoveAllListeners();
            rankingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.AddListener(RankingPanelOff_01);
            settingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.RemoveAllListeners();
            settingPanel01.transform.Find("XButton").GetComponent<Button>().onClick.AddListener(SettingPanelOff_01);
            keySettingText01 = settingPanel01.transform.Find("Setting").transform.Find("KeySet").transform.Find("KeySetText").GetComponent<Text>();

            PlayerPrefs.SetInt(keySet, (int)KeyCode.LeftControl);

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


            roomListPanel02 = canvas.transform.Find("RoomListPanel").gameObject;

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

            distance03 = rDVindex03.transform.Find("Distance").GetComponent<Text>();

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
            SetText(roundText05, GameManager.Instance().GetRound().ToString());
            SetText(gameOverTimeText05, TimeManager.Instance().GetCurTime().ToString());
        }
        #endregion

        #region Scene06
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            ground = GameObject.Find("Ground");

            playableButton_ReStart06 = ground.transform.Find("RePlay").gameObject;
            playableButton_BackToLobby06 = ground.transform.Find("BacktoLobby").gameObject;
            playableButton_BackToMain06 = ground.transform.Find("Main").gameObject;

            clearTimeText06 = ground.transform.Find("FinalRecord").transform.Find("FinalTimeTxt").GetComponent<TextMeshPro>();
            storageLvText06 = ground.transform.Find("Finalstorage").transform.Find("FinalStorageTxt").GetComponent<TextMeshPro>();
            dynamiteLvText06 = ground.transform.Find("FinalDynamite").transform.Find("FinalDynamiteTxt").GetComponent<TextMeshPro>();
            productionLvText06 = ground.transform.Find("FinalProduction").transform.Find("FinalProductionTxt").GetComponent<TextMeshPro>();
            watertankLvText06 = ground.transform.Find("FinalWaterTank").transform.Find("FinalWaterTankTxt").GetComponent<TextMeshPro>();
            
            SetText(clearTimeText06, TimeManager.Instance().GetCurTime().ToString());
            SetText(dynamiteLvText06, StateManager.Instance().dynamiteMachines.Count.ToString());
            SetText(productionLvText06, StateManager.Instance().productionMachines.Count.ToString());
            SetText(watertankLvText06, StateManager.Instance().waterTanks.Count.ToString());

        }
        #endregion
    }
}