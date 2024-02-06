
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
            PlayBGM();
            SceneInit
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

        public void SceneInit()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 6)
            {
                UIManager.Instance().clearTimeText06.text = ((int)TimeManager.Instance().PrevTime).ToString();
            }
        }
    }
}

