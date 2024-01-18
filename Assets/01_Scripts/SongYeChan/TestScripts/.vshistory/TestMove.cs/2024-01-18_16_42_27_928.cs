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
        float y = Input.GetAxis("Mouse Y");
        float x = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.Mouse1))
        {
            // �÷��̾� ��ü ȸ��
            transform.Rotate(new Vector3(0, x, 0) * Time.deltaTime * 50f);
            Camera.main.transform.Rotate(new Vector3(y, -x, 0) * Time.deltaTime * 50f);
        }

        // ĳ���� ��Ʈ�ѷ��� ������ ó��
        Vector3 moveDirection = new Vector3(moveX, 0, moveY);
        characterController.Move(moveDirection * Time.deltaTime * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            trackManager.TrackCreate(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }
}
