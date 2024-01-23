using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameManager;

public class UIManager_LeeYuJoung : MonoBehaviour
{
    public enum PlayableButtonInfo
    {
        GAME_START,
        GAME_EXIT,
        RANKING,
        SETTING
    }
    public PlayableButtonInfo myInfo;

    #region Instance
    private static UIManager_LeeYuJoung instance;
    public static UIManager_LeeYuJoung Instance()
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

    /// <summary>
    /// 플레이어가 해당 버튼에 머물때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    public void PlayAbleButton_OnStay(PlayableButtonInfo _info)
    {
        switch (_info)
        {
            case PlayableButtonInfo.GAME_START:
                ActiveAndDeActive(loginPanel, ground);
                break;
            case PlayableButtonInfo.GAME_EXIT:
                break;
            case PlayableButtonInfo.RANKING:
                break;
            case PlayableButtonInfo.SETTING:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 플레이어가 해당 버튼에서 액션키버튼을 눌렀을때 해야할 함수 호출
    /// </summary>
    /// <param name="_info">PlayableButton 종류</param>
    public void PlayAbleButton_OnHit(PlayableButtonInfo _info)
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

    /// <summary>
    /// 각씬 별로 panel및 playablebutton 초기화
    /// </summary>
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

            //playableButton_GameStart.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.GAME_START;
            //playableButton_Ranking.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.RANKING;
            //playableButton_Setting.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.SETTING;
            //playableButton_GameExit.GetComponent<PlayableButtonInfo>().myInfo = PlayableButtonInfo.GAME_EXIT;

            loginPanel = canvas.transform.Find("LoginPanel").gameObject;
            settingPanel = canvas.transform.Find("SettingPanel").gameObject;
            loginFailPanel = canvas.transform.Find("LoginFailPanel").gameObject;
        }
        // --------------------------------------------------------------------------------------

    }
}
