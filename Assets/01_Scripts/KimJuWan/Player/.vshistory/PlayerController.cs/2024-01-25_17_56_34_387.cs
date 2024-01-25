using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
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
public class PlayerController : MonoBehaviourPunCallbacks
{

    private float moveSpeed = 10f;

    Vector3 moveDirection;
    //Rigidbody rb;
    PlayerManager playerManager;

    public bool isPick;
    public bool isWorking;

    public float currentTime = 0f;
    public float spaceTime = .2f;


    public string playableButtonTagName = "PlayableButton";
    private PhotonView pv;
    private TeamManager teamManager;

    private bool isReady = false;


    // 시작

    void Start()
    {
        pv = GetComponent<PhotonView>();
        playerManager = GetComponent<PlayerManager>();
        if (pv != null && pv.IsMine)
        {
            teamManager = GameObject.Find("TeamManager")?.GetComponent<TeamManager>();
        }
        //rb = GetComponent<Rigidbody>();

    }

    // 업데이트
    void FixedUpdate()
    {
        if (pv == null || (pv != null && pv.IsMine))
        {
            if (!isWorking)
                PlayerMove();
        }
    }

    void CheckPlayableButton_OnStay()
    {
        if (pv == null || (pv != null && pv.IsMine))
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag != null && hit.transform.CompareTag(playableButtonTagName))
                {
                    //UIManager_LeeYuJoung.Instance().PlayAbleButton_OnStay(hit.transform.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo);
                    UIManager.Instance().PlayAbleButton_OnStay(hit.transform.GetComponent<PlayableButtonInfo>().myInfo);
                }
                if (isReady)
                {
                    if (!hit.transform.CompareTag(playableButtonTagName))
                    {
                        Debug.Log("벗어남");
                        SetIsReady(false);
                    }
                }
            }
        }


    }

    void CheckPlayableButton_OnHit()
    {
        if (pv == null || (pv != null && pv.IsMine))
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag != null && hit.transform.CompareTag(playableButtonTagName))
                {
                    //UIManager_LeeYuJoung.Instance().PlayAbleButton_OnHit(hit.transform.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo);
                    UIManager.Instance().PlayAbleButton_OnHit(hit.transform.GetComponent<PlayableButtonInfo>().myInfo);
                }
            }
        }


    }

    private void Update()
    {
        if (pv == null || (pv != null && pv.IsMine))
        {
            if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.None) && Input.GetKeyDown(KeyCodeInfo.myActionKeyCode))
            {
                //스페이스바 눌렀을때 실행
                CheckPlayableButton_OnHit();
            }
            else if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.None))
            {
                //플레이어가 스테이하면 실행
                CheckPlayableButton_OnStay();
            }

            if (Input.GetKey(KeyCodeInfo.myActionKeyCode))
            {
                currentTime += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCodeInfo.myActionKeyCode))
            {
                playerManager.CollectIngredient();
                currentTime = 0;
            }
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

        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            Vector3 moveDirection = new Vector3(h, 0f, v);
            moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

            transform.position += moveDirection;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    public bool GetIsReady()
    {
        if (pv.IsMine)
        {
            return isReady;
        }
        return false;
    }

    public void SetIsReady(bool _isReady)
    {
        if (pv.IsMine)
        {
            Debug.Log("ready Set");
            this.isReady = _isReady;
            teamManager.SetReadyUserCount(_isReady);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (pv.Owner == otherPlayer)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (photonView.IsMine)
        {
            
        }
    }
}