using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 시작
    void Start()
    {
        
    }

    // 업데이트
    void Update()
    {
        
    }

    void UIRayCast()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.up, out hit))
        {

        }
    }
}
