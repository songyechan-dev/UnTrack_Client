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

    // ������Ʈ
    void Update()
    {

    }

    /// <summary>
    /// �÷��̾ �ش� ��ư�� �ӹ��� �ؾ��� �Լ� ȣ��
    /// </summary>
    /// <param name="_info">PlayableButton ����</param>
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
    /// �÷��̾ �ش� ��ư���� �׼�Ű��ư�� �������� �ؾ��� �Լ� ȣ��
    /// </summary>
    /// <param name="_info">PlayableButton ����</param>
    public void PlayAbleButton_OnHit(PlayableButtonInfo _info)
    {

    }
    /// <summary>
    /// Ư�� ������Ʈ�� Active�� ���ų� �Ҵ�
    /// </summary>
    /// <param name="_activeGameObject">SetActive true�� �ؾߵ� ��ü</param>
    /// <param name="_deActiveGameObject">SetActive false�� �ؾߵ� ��ü</param>
    public void ActiveAndDeActive(GameObject _activeGameObject, GameObject _deActiveGameObject)
    {
        _activeGameObject.SetActive(true);
        _deActiveGameObject.SetActive(false);
    }

    /// <summary>
    /// Ư�� ������Ʈ�� active�� �Ѱ�, Ư�� ������Ʈ���� active ������
    /// </summary>
    /// <param name="_activeGameObject">SetActive true�� �ؾߵ� ��ü</param>
    /// <param name="_deActiveGameObjects">SetActive false�� �ؾߵ� ��ü��</param>
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
    /// ���� ���� panel�� playablebutton �ʱ�ȭ
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
