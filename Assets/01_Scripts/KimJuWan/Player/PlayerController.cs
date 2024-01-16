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
    public bool isPick;
    public GameObject grabedObject;
    


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

    //플레이어 이동
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


    
    void Pick()
    {
        if (isPick)
        {
            if (Input.GetKeyDown(KeyCode.Z) && grabedObject !=null)
            {
                //아이템을 플레이어의 자식 객체로 포함
                grabedObject.transform.parent = transform;


                isPick = true;
                //플레이어가 집은 아이템이 재료인 경우에만 인벤토리에 데이터 저장(01.16 수정)
                if (grabedObject.name == "Wood" || grabedObject.name == "Steel")
                {
                    inventoryManager.SaveInventory();
                    
                }

            }
        }
        
    }

    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.C) && isPick)
        {

            transform.DetachChildren();
            grabedObject = null;
            isPick = false;


        }
    }
}
