using LeeYuJoung;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KLSDev2023
{
    public class SceneManager: MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance().Init();
            PlayBGM();
        }

        public void NextScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Test()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3);
            }
        }

        public void PlayBGM()
        {
            AudioManager.Instnce().PlayBGM(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}

