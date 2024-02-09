using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class ChatPositionManager : MonoBehaviour
{
    private void Update()
    {
        Quaternion parentRotation = transform.parent.rotation;
        Quaternion inverseParentYRotation = Quaternion.Euler(0f, -parentRotation.eulerAngles.y, 0f);
        transform.localRotation = inverseParentYRotation;
    }
}
