using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private TrackManager trackManager;
    float moveX => Input.GetAxis("Horizontal");
    float moveY => Input.GetAxis("Vertical");
    float moveSpeed = 10f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    void Update()
    {
        float y = Input.GetAxis("Mouse Y");
        float x = Input.GetAxis("Mouse X");

        

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Camera.main.transform.Rotate(new Vector3(-y, x, 0) * Time.deltaTime * 50f);
        }
        characterController.Move(new Vector3(moveX, 0, moveY) * Time.deltaTime * moveSpeed);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            trackManager.TrackCreate(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

    }
}
