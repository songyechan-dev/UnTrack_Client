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
    public InventoryManager inventoryManager;
    public bool isPickWood;
    public bool isPickSteel;
    public bool isDrop;
    public List<GameObject> pickedWoods = new List<GameObject>();
    public List<GameObject> pickedSteels = new List<GameObject>();
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
        if (isPickWood)
        {
            PickWoods();
            isPickSteel = false;
        }
        if (isPickSteel)
        {
            PickSteels();
            isPickWood = false;
        }

    }

    //플레이어 이동
    public void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection.Set(h, 0f, v);

        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
        if (!(h == 0 && v == 0))
        {
            rb.MovePosition(transform.position + moveDirection);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

    }

    public void PickWoods()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //아이템을 플레이어의 자식 객체로 포함
            if (nearObject.name == "Wood")
                pickedWoods.Add(nearObject);


            for (int i = 0; i < pickedWoods.Count; i++)
            {
                pickedWoods[i].transform.parent = transform;
                pickedWoods[i].transform.position = transform.localPosition;
            }

            inventoryManager.SavePlayerInventory();


        }
        nearObject = null;

    }

    public void PickSteels()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //아이템을 플레이어의 자식 객체로 포함
            if (nearObject.name == "Steel")
                pickedSteels.Add(nearObject);

            for (int i = 0; i < pickedSteels.Count; i++)
            {
                pickedSteels[i].transform.parent = transform;
                pickedSteels[i].transform.position = transform.localPosition;
            }

            inventoryManager.SavePlayerInventory();
        }
        nearObject = null;

    }

    public void Drop()
    {
        transform.DetachChildren();
    }

}