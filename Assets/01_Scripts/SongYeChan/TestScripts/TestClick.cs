using LeeYuJoung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestClick : MonoBehaviour
{
    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GameClear()
    {
        GameManager.Instance().GameClear();
    }

    public void AddVolt()
    {
        StateManager.Instance().SetVolt(true, 1);
    }
}
