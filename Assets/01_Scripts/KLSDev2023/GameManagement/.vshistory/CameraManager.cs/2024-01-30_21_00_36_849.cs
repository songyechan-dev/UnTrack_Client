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
            float targetX = playerTransform.position.x;

            // X 좌표를 최소값과 최대값 사이로 제한
            targetX = Mathf.Clamp(targetX, minX, maxX);
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(targetX, currentPos.y, currentPos.z);
        }
    }
}
