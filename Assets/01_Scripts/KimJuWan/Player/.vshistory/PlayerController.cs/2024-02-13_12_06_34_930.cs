using DG.Tweening;
using ExitGames.Client.Photon;
using LeeYuJoung;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    //Player 애니메이션용 STATE
    public enum PLAYERSTATE
    {
        IDLE = 0,
        WALK = 1,
        PICKUP = 2,
        DROP = 3,
        EQUIPMENTACTION = 4,

        PICK = 5

    }
    [SerializeField]
    private float moveSpeed = 2f;

    Vector3 moveDirection;
    //Rigidbody rb;
    PlayerManager playerManager;
    public PLAYERSTATE playerState;
    public Animator playerAnim;
    public float dropCurTime = 0;
    public float pickCurTime = 0;
    public bool isPick;
    public bool isWorking;
    public int animLoopCnt = 3;
    public float currentTime = 0f;
    public float spaceTime = .2f;
    public AudioSource playerAudio;

    public string playableButtonTagName = "PlayableButton";
    private PhotonView pv;
    public TeamManager teamManager;
    public Transform sensor;

    private bool isReady = false;
    private bool isExit = false;
    private Dictionary<int,bool> isReady_Scene = new Dictionary<int,bool>();
    public Material outlineMaterial;

    // 시작

    void Start()
    {
        pv = GetComponent<PhotonView>();
        playerManager = GetComponent<PlayerManager>();
        sensor = transform.Find("Sensor").transform;
        playerAnim = transform.Find("Duck").GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        if (pv != null && pv.IsMine)
        {
            if (GameObject.Find("ChatManager") != null)
            {
                GameObject.Find("ChatManager").GetComponent<ChatManager>().player = gameObject;
            }
            if (GameObject.Find("UIManager") != null && GameObject.Find("UIManager").GetComponent<UIManager>().playerController == null)
            {
                GameObject.Find("UIManager").GetComponent<UIManager>().playerController = gameObject.GetComponent<PlayerController>();
            }
            sensor = transform.Find("Sensor").transform;
            playerManager = GetComponent<PlayerManager>();
            teamManager = GameObject.Find("TeamManager")?.GetComponent<TeamManager>();

        }
        //rb = GetComponent<Rigidbody>();
        if (pv == null)
        {
            playerAnim = transform.Find("Duck").GetComponent<Animator>();
        }

    }

    // 업데이트
    void FixedUpdate()
    {

        if (pv == null || (pv != null && pv.IsMine && !isExit))
        {
            if (!isWorking && pv == null)
            {
                PlayerMove();
            }
            else if (!isWorking && pv != null && pv.IsMine)
            {
                if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.Play))
                {
                    float newX = Mathf.Clamp(transform.position.x, MapInfo.startPosition.x, MapInfo.endPositionX - 1f);
                    float newZ = Mathf.Clamp(transform.position.z, MapInfo.startPosition.z, MapInfo.endPositionZ - 1f);
                    if (newX == Mathf.Clamp(newX, MapInfo.startPosition.x, MapInfo.endPositionX - 1f) &&
                        newZ == Mathf.Clamp(newZ, MapInfo.startPosition.z, MapInfo.endPositionZ - 1f))
                    {
                        transform.position = new Vector3(newX, transform.position.y, newZ);
                        PlayerMove();
                    }
                    else
                    {
                        return;
                    }
                }
                else if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.None))
                {
                    PlayerMove();
                }

            }

        }
        else if (PhotonNetwork.IsMasterClient && isExit)
        {
            if (isReady_Scene.ContainsKey(SceneManager.GetActiveScene().buildIndex) && isReady_Scene[SceneManager.GetActiveScene().buildIndex])
            {
                teamManager.SetReadyUserCount(false);
            }
            PhotonNetwork.Destroy(gameObject);
        }

    }

    void CheckPlayableButton_OnStay()
    {
        if (pv == null || (pv != null && pv.IsMine))
        {
            Ray ray = new Ray(sensor.transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,3f))
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
            Ray ray = new Ray(sensor.transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,3f))
            {
                Debug.Log("Hit실행");
                if (hit.transform.tag != null && hit.transform.CompareTag(playableButtonTagName))
                {
                    //UIManager_LeeYuJoung.Instance().PlayAbleButton_OnHit(hit.transform.GetComponent<PlayableButtonInfo_LeeYuJoung>().myInfo);
                    UIManager.Instance().PlayAbleButton_OnHit(hit.transform.GetComponent<PlayableButtonInfo>());
                    Debug.Log(hit.transform.GetComponent<PlayableButtonInfo>().myInfo);
                }
            }
        }


    }

   

    private void Update()
    {
        //플레이어 애니메이션 재생
        if (pv == null || (pv != null && pv.IsMine))
        {
            if (PhotonNetwork.IsConnected)
            {
                switch (playerState)
                {
                    case PLAYERSTATE.IDLE:
                        playerAnim.SetInteger("PLAYERSTATE", 0);
                        ChangePlayerState(playerState);
                        if (isWorking)
                        {
                            AudioManager.Instnce().PlaySFX(playerAudio, SOUNDTYPE.DIG);
                            playerState = PLAYERSTATE.EQUIPMENTACTION;
                            ChangePlayerState(playerState);
                        }

                        break;
                    case PLAYERSTATE.WALK:
                        playerAnim.SetInteger("PLAYERSTATE", 1);
                        

                        ChangePlayerState(playerState);
                        if (isWorking)
                        {
                            playerState = PLAYERSTATE.EQUIPMENTACTION;
                            ChangePlayerState(playerState);
                        }
                        if (isPick)
                        {
                            playerState = PLAYERSTATE.PICK;
                            ChangePlayerState(playerState);
                        }
                        if (Input.GetKeyDown(KeyCodeInfo.myActionKeyCode))
                        {
                            if (!isPick)
                            {
                                playerState = PLAYERSTATE.PICKUP;
                                ChangePlayerState(playerState);
                            }

                            else
                            {
                                playerState = PLAYERSTATE.DROP;
                                ChangePlayerState(playerState);
                            }

                        }
                        break;
                    case PLAYERSTATE.PICKUP:
                        playerAnim.SetInteger("PLAYERSTATE", 2);
                        ;
                        ChangePlayerState(playerState);
                        break;
                    case PLAYERSTATE.DROP:
                        playerAnim.SetInteger("PLAYERSTATE", 3);
                        ChangePlayerState(playerState);
                        dropCurTime += Time.deltaTime;
                        AnimatorClipInfo[] curClipInfo;
                        curClipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
                        if (dropCurTime > curClipInfo[0].clip.length)
                        {
                            dropCurTime = 0;
                            playerState = PLAYERSTATE.IDLE;
                            ChangePlayerState(playerState);
                        }
                        break;
                    case PLAYERSTATE.EQUIPMENTACTION:
                        playerAnim.SetInteger("PLAYERSTATE", 4);
                        
                        ChangePlayerState(playerState);
                        if (!isWorking)
                        {
                            playerState = PLAYERSTATE.IDLE;
                            ChangePlayerState(playerState);
                        }
                        break;

                    case PLAYERSTATE.PICK:
                        playerAnim.SetInteger("PLAYERSTATE", 5);
                        ChangePlayerState(playerState);
                        if (!isPick)
                        {
                            playerState = PLAYERSTATE.DROP;
                            ChangePlayerState(playerState);
                        }
                        break;
                }
            }

            if (transform.position.y <= -1f)
            {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            }
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
            if (Input.GetKeyUp(KeyCodeInfo.myActionKeyCode) && GameManager.Instance().gameMode.Equals(GameManager.GameMode.Play))
            {
                playerManager.CollectIngredient();
                Debug.Log("실행됨");
                currentTime = 0;
            }
            else if (GameManager.Instance().gameMode.Equals(GameManager.GameMode.Play))
            {
                CheckObject();
            }
        }

        if (pv == null)
        {
            switch (playerState)
            {
                case PLAYERSTATE.IDLE:
                    playerAnim.SetInteger("PLAYERSTATE", 0);
                    if (isWorking)
                    {
                        AudioManager.Instnce().PlaySFX(playerAudio, SOUNDTYPE.DIG);
                        playerState = PLAYERSTATE.EQUIPMENTACTION;
                    }
                    if (isPick)
                    {
                        playerState = PLAYERSTATE.PICK;
                    }
                    if (Input.GetKeyDown(KeyCodeInfo.myActionKeyCode))
                    {
                        if (!isPick)
                            playerState = PLAYERSTATE.PICKUP;
                        else
                            playerState = PLAYERSTATE.DROP;
                    }
                    
                    break;
                case PLAYERSTATE.WALK:
                    playerAnim.SetInteger("PLAYERSTATE", 1);
                    
                    if (isWorking)
                    {
                        playerState = PLAYERSTATE.EQUIPMENTACTION;
                    }
                    if (Input.GetKeyDown(KeyCodeInfo.myActionKeyCode))
                    {
                        if (!isPick)
                        {
                            playerState = PLAYERSTATE.PICKUP;
                            
                        }

                        else
                        {
                            playerState = PLAYERSTATE.DROP;
                            
                        }

                    }
                    break;
                case PLAYERSTATE.PICKUP:
                    playerAnim.SetInteger("PLAYERSTATE", 2);
                    AudioManager.Instnce().PlaySFX(playerAudio, SOUNDTYPE.PICKUP);
                    break;
                case PLAYERSTATE.DROP:
                    playerAnim.SetInteger("PLAYERSTATE", 3);
                    dropCurTime += Time.deltaTime;
                    AnimatorClipInfo[] curClipInfo;
                    curClipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
                    if (dropCurTime > curClipInfo[0].clip.length)
                    {
                        dropCurTime = 0;
                        playerState = PLAYERSTATE.IDLE;
                    }
                    break;
                case PLAYERSTATE.EQUIPMENTACTION:
                    playerAnim.SetInteger("PLAYERSTATE", 4);
                    AudioManager.Instnce().PlaySFX(playerAudio, SOUNDTYPE.DIG);
                    if (!isWorking)
                    {
                        playerState = PLAYERSTATE.IDLE;
                    }
                    break;

                case PLAYERSTATE.PICK:
                    playerAnim.SetInteger("PLAYERSTATE", 5);
                    if (!isPick)
                    {
                        playerState = PLAYERSTATE.DROP;
                    }
                    break;

            }
        }


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    playerManager.ButtonClick();
        //}
    }

    GameObject tempObject;
    List<Material> prevMaterials = new List<Material>();
    void CheckObject()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("PickSlot"));
        RaycastHit hit;
        Ray ray = new Ray(sensor.position, -sensor.up);

        if (Physics.Raycast(ray, out hit, 3f, layerMask))
        {
            if (tempObject != null)
            {
                MeshRenderer renderer = tempObject.transform.GetChild(0)?.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material[] materials = renderer.materials;
                    materials[0] = prevMaterials[0];
                    for (int i = 0; i < prevMaterials.Count; i++)
                    {

                    }
                }
            }
            tempObject = hit.transform.gameObject;
            Debug.Log("충돌됨");

            // 충돌된 객체의 모든 MeshRenderer를 가져온다
            MeshRenderer meshRenderer = hit.transform.GetChild(0)?.GetComponent<MeshRenderer>();

            // 충돌된 객체가 MeshRenderer를 가지고 있다면
            if (meshRenderer!= null)
            {
                prevMaterials.Clear();
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    prevMaterials.Add(meshRenderer.materials[i]);
                }
                meshRenderer.material = outlineMaterial;
            }
        }
    }




    private void ChangePlayerState(PLAYERSTATE newState)
    {
        playerState = newState;
        photonView.RPC("RPC_ChangePlayerState", RpcTarget.Others, (int)newState,photonView.ViewID);
    }

    [PunRPC]
    private void RPC_ChangePlayerState(int newState,int _viewID)
    {
        if (playerAnim == null) return;
        PlayerController _playerController = PhotonView.Find(_viewID).GetComponent<PlayerController>();
        _playerController.playerState = (PLAYERSTATE)newState;
        switch ((PLAYERSTATE)newState)
            {
                case PLAYERSTATE.IDLE:
                    playerAnim.SetInteger("PLAYERSTATE", 0);
                    break;
                case PLAYERSTATE.WALK:
                    playerAnim.SetInteger("PLAYERSTATE", 1);
                    
                    break;
                case PLAYERSTATE.PICKUP:
                    playerAnim.SetInteger("PLAYERSTATE", 2);
                    
                break;
                case PLAYERSTATE.DROP:
                    playerAnim.SetInteger("PLAYERSTATE", 3);
                    break;
                case PLAYERSTATE.EQUIPMENTACTION:
                    playerAnim.SetInteger("PLAYERSTATE", 4);
                    break;

                case PLAYERSTATE.PICK:
                    playerAnim.SetInteger("PLAYERSTATE", 5);
                    break;

            }
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
            playerState = PLAYERSTATE.WALK;
            AudioManager.Instnce().PlaySFX(playerAudio, 0);
        }
        else
        {
            playerState = PLAYERSTATE.IDLE;
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
            if (!isReady_Scene.ContainsKey(SceneManager.GetActiveScene().buildIndex))
            {
                isReady_Scene.Add(SceneManager.GetActiveScene().buildIndex, _isReady);
            }
            else
            {
                isReady_Scene[SceneManager.GetActiveScene().buildIndex] = _isReady;
            }
            teamManager.SetReadyUserCount(_isReady);
        }

    }



    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (pv.CreatorActorNr == otherPlayer.ActorNumber)
        {
            if (photonView.IsMine)
            {
                isExit = true;
            }
        }
    }
    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    if (pv.IsMine)
    //    {
    //        PhotonNetwork.Destroy(gameObject);
    //    }
    //}

    //private void OnApplicationQuit()
    //{
    //    if (pv.IsMine)
    //    {
    //        Debug.Log("나감");
    //        pv.RPC("Delete", RpcTarget.Others);
    //    }
    //}

    //[PunRPC]
    //void Delete()
    //{
    //    Debug.Log("여기호출됨");
    //    PhotonNetwork.Destroy(gameObject);
    //}
    //void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == (int)SendDataInfo.Info.PLAYER_STATE)
    //    {
    //        object[] receivedData = (object[])photonEvent.CustomData;
    //        PLAYERSTATE _playerState = (PLAYERSTATE)((int)receivedData[0]);
    //        int viewID = (int)receivedData[1];
    //        PlayerController _playerController = PhotonView.Find(viewID).GetComponent<PlayerController>();
    //        _playerController.playerState = _playerState;
    //    }
    //}

    //void OnEnable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    //}

    //void OnDisable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    //}

    

}