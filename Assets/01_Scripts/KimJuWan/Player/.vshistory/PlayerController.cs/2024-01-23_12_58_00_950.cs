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
    
    private float moveSpeed = 20f;
    
    Vector3 moveDirection;
    //Rigidbody rb;
    PlayerManager playerManager;
     
    public bool isPick;
    public bool isWorking;

    public float currentTime = 0f;
    public float spaceTime = .2f;

    public int keyCode = 0;
    public string playableButtonTagName = "PlayableButton";
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
        //rb = GetComponent<Rigidbody>();
        
    }

    // 업데이트
    void FixedUpdate()
    {
        if(!isWorking)
            PlayerMove();
        //TODO 김주완 0118: Space 키 변수화 하기(단축키 설정) -> 0119 완료

        if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.None))
        {
            //플레이어가 스테이하면 떠야하는것
            CheckPlayableButton_OnStay();
        }
        if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.None) && Input.GetKeyDown(keys[keyCode]))
        {
            //스페이스바 눌렀을때 떠야하는것
            CheckPlayableButton_OnHit();
        }
    }

    void CheckPlayableButton_OnStay()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != null && hit.transform.CompareTag(playableButtonTagName))
            {
                Debug.Log("있음");
            }
        }
    }

    void CheckPlayableButton_OnHit()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != null && hit.transform.CompareTag(playableButtonTagName))
            {
                Debug.Log("있음");
            }
        }
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
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;  
        }       
    }  
}