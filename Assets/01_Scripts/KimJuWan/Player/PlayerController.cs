using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 6f;
    
    Vector3 moveDirection;
    Rigidbody rb;
    PlayerManager playerManager;
    public InventoryManager inventoryManager; 
    private bool isPick;
    public GameObject nearObject;


    // 시작
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
    }

    // 업데이트
    void FixedUpdate()
    {
        PlayerMove();
        Pick();
        Drop();
    }

    public void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection.Set(h, 0f, v);

        moveDirection =  moveDirection.normalized * moveSpeed * Time.deltaTime;
        if (!(h == 0 && v == 0))
        {
            rb.MovePosition(transform.position + moveDirection);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearObject = null;
        }
    }

    void Pick()
    {
        if (Input.GetKeyDown(KeyCode.Z) && nearObject != null)
        {
            nearObject.transform.parent = transform;
            
            isPick = true;
            if(nearObject.name == "Wood")
            {
                inventoryManager.PlayerInventoryWood();
            }
            if(nearObject.name == "Steel")
            {
                inventoryManager.PlayerInventorySteel();
            }
        }
    }

    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.C) && isPick)
        {

            nearObject.transform.parent = null;
            isPick = false;


        }
    }
}
