using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private TrackManager trackManager;
    private CharacterController characterController;
    float moveX => Input.GetAxis("Horizontal");
    float moveY => Input.GetAxis("Vertical");
    float moveSpeed = 10f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.Mouse1))
        {
            // 마우스 X 축 입력에 따라 플레이어 회전
            transform.Rotate(new Vector3(0, x, 0) * Time.deltaTime * 50f);
        }

        // 현재 플레이어가 바라보는 방향으로 이동
        Vector3 moveDirection = transform.forward * moveY + transform.right * moveX;
        characterController.Move(moveDirection * Time.deltaTime * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            trackManager.TrackCreate(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }
}
