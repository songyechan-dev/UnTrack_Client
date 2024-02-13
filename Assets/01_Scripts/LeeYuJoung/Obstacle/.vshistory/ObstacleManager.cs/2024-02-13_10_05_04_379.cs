using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System;
using Photon.Pun;
using DG.Tweening;
using ExitGames.Client.Photon;
using static GameManager;
using Unity.VisualScripting;


public class ObstacleManager : MonoBehaviour
{
    public string dataPath;
    public int obstacleType;

    private string equipmentType;
    private string generateItem;
    private float workTime;
    private float currentTime = 0;

    // 현재 Obstacle을 Player가 작업 중인지 확인
    public bool isWorking = false;
    public RaycastHit hit;

    void Start()
    {
        ObstacleJsonLoad(dataPath);
    }

    // 현재 Obstacle의 작업 가능 여부 확인
    public void ObstacleWorking(string _equipment, PlayerController _player)
    {
        if (isWorking || !equipmentType.Equals(_equipment) || _player.gameObject.transform.Find("PickSlot").GetComponent<InventoryManager>().itemNum <= 0)
        {
            Debug.Log("::: 이미 작업 중 이거나 장비가 맞지 않음 혹은 이미 내려놓은상태임:::");
            return;
        }
        else
        {
            CheckObstacleHeight(_player);
        }
    }

    // 위에 Obstacle이 더 있는지 확인
    public void CheckObstacleHeight(PlayerController _player)
    {
        Ray ray = new Ray(transform.position, transform.up);

        if(Physics.Raycast(ray, out hit, 1.0f))
        {
            Debug.Log("위에 장애물이 있습니다");
            if (hit.transform.tag != null && hit.transform.tag.Equals("Obstacle"))
            {
                hit.transform.GetComponent<ObstacleManager>().CheckObstacleHeight(_player);
            }
            else
            {
                Debug.Log("맨 위 장애물 입니다");
                isWorking = true;
                _player.isWorking = true;
                StartCoroutine(ObstacleDelete(_player));
            }
            
        }
        else
        {
            Debug.Log("맨 위 장애물 입니다");
            isWorking = true;
            _player.isWorking = true;
            StartCoroutine(ObstacleDelete(_player));
        }
    }

    // Obstacle 제거
    IEnumerator ObstacleDelete(PlayerController _player)
    {
        transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 6.0f);


        int loopNum = 0;

        while (isWorking)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;

            if (currentTime > workTime)
            {
                currentTime = 0;
                isWorking = false;

                _player.isWorking = false;
                GenerateIngredient(_player);
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                if (photonView.IsMine || PhotonNetwork.IsMasterClient)
                {
                    if (!photonView.IsMine)
                    {
                        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    // 마스터 클라이언트에서만 삭제를 시도
                    PhotonNetwork.Destroy(gameObject);
                }

            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
     

    }

    // 재료 생성
    public void GenerateIngredient(PlayerController _player)
    {
        Debug.Log("Generate ::: " + generateItem);
        QuestManager.Instance().UpdateProgress(generateItem, 1); 
        GameObject _object = PhotonNetwork.Instantiate(generateItem, new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z)-transform.forward, transform.rotation);
        _object.name = generateItem;
    }

    // TODO : 이유정 2024.01.15 ObstacleManager.cs ObstacleJsonLoad(string path)
    void ObstacleJsonLoad(string _path)
    {
        TextAsset json = (TextAsset)Resources.Load(_path);
        string jsonStr = json.text;

        var jsonData = JSON.Parse(jsonStr);

        equipmentType = jsonData[obstacleType]["EQUIPMENT_TYPE"];
        workTime = (int)jsonData[obstacleType]["WORK_TIME"];
        generateItem = jsonData[obstacleType]["GENERATE"];
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.OB_ANIMATION)
        {
            // 다른 플레이어들이 호출한 RPC로 미터 값을 받음
            object[] receivedData = (object[])photonEvent.CustomData;
            int viewID = (int)receivedData[0];
            GameObject go = PhotonView.Find(viewID).gameObject;

            UIManager.Instance().SetText(UIManager.Instance().distance03, (int)(receivedMeter * 100) + "m");
        }
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
