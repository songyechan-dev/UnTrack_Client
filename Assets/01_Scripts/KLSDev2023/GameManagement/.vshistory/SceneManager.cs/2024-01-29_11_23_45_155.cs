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
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
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

