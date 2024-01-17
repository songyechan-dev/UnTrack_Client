using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class PlayerController : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        IDLE = 0,
        WALK,
        PICKUP,
        AX,
        PICK
    }
    private float moveSpeed = 6f;
    
    Vector3 moveDirection;
    Rigidbody rb;
    PlayerManager playerManager;
    //public InventoryManager inventoryManager; 
    public bool isPick;
    
    public bool isDrop;
    public Transform pickSlot;
    public Stack<GameObject> items;
    public GameObject nearObject;
    //public ItemManager itemManager;

    // 시작
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        pickSlot = transform.GetChild(0).transform;
        pickSlot.localPosition = transform.position;
    }

    // 업데이트
    void FixedUpdate()
    {
        PlayerMove();
        if (isPick)
            Pick(nearObject);
        
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
    
    public void Pick(GameObject pickedObject)
    {
        
       if (Input.GetButtonDown("Pick"))
       {    //테스트
            items.Push(pickedObject);
            
       }
          
    }

    

    public void Drop()
    {
        isPick = false;
    }
    
}