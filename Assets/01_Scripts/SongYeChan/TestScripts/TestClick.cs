using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestClick : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("여기서 호출됨");
    }

    // 시작
    void Start()
    {
        
    }

    // 업데이트
    void Update()
    {
        
    }
}
