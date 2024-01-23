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
        
    }

    public void PlayAbleButton_OnHit(PlayableButtonInfo.Info _info)
    {

    }

    public void ChangeKeyCode()
    {
        playerController.keyCode++;
        if(playerController.keyCode >2)
        {
            playerController.keyCode = 0;
        }
    }

    public void Init()
    {
        playerController = GetComponent<PlayerController>();
        ground = GameObject.Find("Ground");
        playableButton_GameStart = ground.transform.Find("GameStart").gameObject;
        playableButton_Ranking = ground.transform.Find("Ranking").gameObject;
        playableButton_Setting = ground.transform.Find("Setting").gameObject;
    }

    //public void MoveToLobby()
    //{
        
    //}
}
