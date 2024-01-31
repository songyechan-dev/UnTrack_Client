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
            targetX = Mathf.Clamp(targetX, minX, maxX);
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(targetX, currentPos.y, currentPos.z);
        }
    }
}
