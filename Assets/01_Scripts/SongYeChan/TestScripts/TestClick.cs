using LeeYuJoung;
using Photon.Pun;
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

    public void AddPlayer()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 20, 0), Quaternion.identity);
    }
}
