using Google.GData.Documents;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;


//Player 애니메이션용 STATE
public enum PLAYERSTATE
{
    IDLE = 0,
    WALK,
    PICKUP,
    DROP,
    EQUIP_AX,
    EQUIP_PICK,
    DYNAMITE,
    BUCKET

}
public class PlayerController : MonoBehaviour
{
    
    private float moveSpeed = 6f;
    
    Vector3 moveDirection;
    Rigidbody rb;
    PlayerManager playerManager;
     
    public bool isPick;

    public float currentTime = 0f;
    public float spaceTime = .2f;

    public int keyCode = 0;
    public List<KeyCode> keys = new List<KeyCode>()
    {
        KeyCode.Space,
        KeyCode.LeftControl,
        KeyCode.LeftShift
    };


    // 시작
   
    void Start()
    {
       
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        
    }

    // 업데이트
    void FixedUpdate()
    {
        PlayerMove();
        //TODO 김주완 0118: Space 키 변수화 하기(단축키 설정) -> 0119 완료
       
    }
    private void Update()
    {
        if (Input.GetKey(keys[keyCode]))
        {
            currentTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(keys[keyCode]))
        {
            playerManager.CollectIngredient();
            currentTime = 0;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    playerManager.ButtonClick();
        //}
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
            transform.position +=moveDirection;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;  
        }
        
    }  
}