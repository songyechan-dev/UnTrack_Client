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
        }
    }
}

