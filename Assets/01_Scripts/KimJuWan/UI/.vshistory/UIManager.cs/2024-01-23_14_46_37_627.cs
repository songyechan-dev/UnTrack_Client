using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameManager;


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

    #region playableButtons
    [Header("PlayableButtons")]
    public GameObject ground;
    public GameObject playableButton_GameStart;
    public GameObject playableButton_Ranking;
    public GameObject playableButton_GameExit;
    public GameObject playableButton_Setting;
    public GameObject canvas;
    [Header("Panel")]
    public GameObject loginPanel;
    public GameObject settingPanel;
    public GameObject loginFailPanel;

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

    // 업데이트
    void Update()
    {
        
    }

    public void PlayAbleButton_OnStay(PlayableButtonInfo.Info _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo.Info.GAME_START:
                ActiveAndDeActive(ground, loginPanel);
                break;
            case PlayableButtonInfo.Info.GAME_EXIT:
                break;
            case PlayableButtonInfo.Info.LANKING:
                break;
            case PlayableButtonInfo.Info.SETTING:
                break;
            default:
                break;
        }
    }

    public void PlayAbleButton_OnHit(PlayableButtonInfo.Info _info)
    {

    }

    public void ActiveAndDeActive(GameObject _deActiveGameObject, GameObject _activeGameObject)
    {
        _activeGameObject.SetActive(true);
        _deActiveGameObject.SetActive(false);
    }

    public void LoginButtonOnClick()
    {
        string user_id = loginPanel.transform.Find("InputID").GetComponent<InputField>().text;
        string user_password = loginPanel.transform.Find("InputPW").GetComponent<InputField>().text;
        StartCoroutine(WebServerManager.LoginCoroutine(user_id, user_password));
    }

    //public void ChangeKeyCode()
    //{
    //    playerController.keyCode++;
    //    if(playerController.keyCode >2)
    //    {
    //        playerController.keyCode = 0;
    //    }
    //}

    public void Init()
    {
        playerController = GetComponent<PlayerController>();
        ground = GameObject.Find("Ground");
        canvas = GameObject.Find("Canvas");
        // --------------------------------------------------------------------------------------
        if (SceneManager.GetActiveScene().name.Equals("01_Intro"))
        {
            playableButton_GameStart = ground.transform.Find("GameStart").gameObject;
            playableButton_Ranking = ground.transform.Find("Ranking").gameObject;
            playableButton_Setting = ground.transform.Find("Setting").gameObject;
            playableButton_GameExit = ground.transform.Find("Quit").gameObject;

            playableButton_GameStart.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.Info.GAME_START;

            loginPanel = canvas.transform.Find("LoginPanel").gameObject;
            settingPanel = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel = canvas.transform.Find("LoginFailPanel").gameObject;
        }
        // --------------------------------------------------------------------------------------
        Debug.Log(SceneManager.GetActiveScene().name);

    }

    //public void MoveToLobby()
    //{
        
    //}
}
