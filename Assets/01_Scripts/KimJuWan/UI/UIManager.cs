using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    
    public PlayerController playerController;
    // 시작
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // 업데이트
    void Update()
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
