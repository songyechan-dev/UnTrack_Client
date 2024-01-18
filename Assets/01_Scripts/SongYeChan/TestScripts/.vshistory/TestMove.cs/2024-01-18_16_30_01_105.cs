using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private TrackManager trackManager;
    float x => Input.GetAxis("Horizontal");
    float y => Input.GetAxis("Vertical");
    float moveSpeed = 10f;

    void Start()
    {
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(x, 0, y).normalized;
        float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.World);
        if (Input.GetMouseButtonDown(0))
        {
            trackManager.TrackCreate(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }
}
