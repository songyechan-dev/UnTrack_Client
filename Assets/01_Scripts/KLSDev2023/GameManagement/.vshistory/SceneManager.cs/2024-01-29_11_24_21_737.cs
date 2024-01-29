using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KLSDev2023
{
    public class SceneManager: MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance().Init();
            //TODO : �׽�Ʈ �ڵ� ���� �ʿ� �ۿ���
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2 && !PhotonNetwork.IsMasterClient)
            {
                NextScene();
            }
        }

        public void NextScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

