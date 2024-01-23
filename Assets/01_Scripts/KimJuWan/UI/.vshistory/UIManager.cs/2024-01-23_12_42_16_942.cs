using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public GameObject gameStart;
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
        playerController = GetComponent<PlayerController>();

    }
    // 시작
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // 업데이트
    void Update()
    {
        
    }

    public void PlayAbleButtonOnStay(PlayableButtonInfo _info)
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

    //public void MoveToLobby()
    //{
        
    //}
}
