using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public Transform playerTransform;
    public float minX = 15f;
    public float maxX = 85f;

    void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어의 현재 위치를 가져옴
            float targetX = playerTransform.position.x;

            // X 좌표를 최소값과 최대값 사이로 제한
            targetX = Mathf.Clamp(targetX, minX, maxX);

            // 현재 카메라 위치를 가져옴
            Vector3 currentPos = transform.position;

            // 플레이어의 X 좌표로만 카메라 위치를 설정
            transform.position = new Vector3(targetX, currentPos.y, currentPos.z);
        }
    }
}

}
