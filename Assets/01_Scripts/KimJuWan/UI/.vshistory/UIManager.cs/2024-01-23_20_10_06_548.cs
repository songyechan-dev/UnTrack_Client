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
    public GameObject canvas;
    public GameObject ground;
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
    #endregion

    #region Scene02
    [Header("PlayableButtons")]
    public GameObject playableButton_GameStart02;
    [Header("Panel")]
    public GameObject RoomListPanel;

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
                ActiveAndDeActive(loginPanel01, ground);
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
        string user_id = loginPanel01.transform.Find("InputID").GetComponent<InputField>().text;
        string user_password = loginPanel01.transform.Find("InputPW").GetComponent<InputField>().text;
        StartCoroutine(WebServerManager.LoginCoroutine(user_id, user_password));
    }

    /// <summary>
    /// 각씬 별로 panel및 playablebutton 초기화
    /// </summary>
    public void Init()
    {
        //playerController = GetComponent<PlayerController>();
        ground = GameObject.Find("Ground");
        canvas = GameObject.Find("Canvas");
        #region Scene01
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
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
            settingPanel01 = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel01 = canvas.transform.Find("LoginFailPanel").gameObject;
        }
        #endregion
        #region Scene02
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            
        }
        #endregion

    }

}
