using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System;
using LeeYuJoung;
using Photon.Pun;
using ExitGames.Client.Photon;
using static GameManager;
using Photon.Realtime;

public class FactoryManager : MonoBehaviourPun
{
    public enum FACTORYTYPE
    {
        ProductionMachine,
        WaterTank,
        DynamiteMachine
    }
    public FACTORYTYPE factoryType;

    public string dataPath;
    public string ingredient_1;
    public int amount_1;
    public string ingredient_2;
    public int amount_2;
    public float generateTime;
    public string generateItem;

    public float fireDeadTime = 50;
    public float currentFireTime = 0;
    public float currentGenerateTime = 0;

    public int currentItemNum = 0;
    public int itemMaxVolume = 5;

    public bool isWorking = false;
    public bool isHeating = false;

    public void Start()
    {
        //Init();
    }

    public void Init()
    {
        if (GameManager.Instance().GetRound() == 1)
        {
            FactoryJsonLoad(dataPath);
        }
        else
        {
            FactoryJsonLoad(dataPath);

            switch (factoryType)
            {
                case FACTORYTYPE.ProductionMachine:
                    if(StateManager.Instance().productionMachines.Count > 0)
                    {
                        currentItemNum = StateManager.Instance().productionMachines[0]["currentItemNum"];
                        itemMaxVolume = StateManager.Instance().productionMachines[0]["itemMaxVolume"];
                        StateManager.Instance().productionMachines.RemoveAt(0);
                    }

                    break;
                case FACTORYTYPE.WaterTank:
                    if(StateManager.Instance().waterTanks.Count > 0)
                    {
                        currentItemNum = StateManager.Instance().waterTanks[0]["currentItemNum"];
                        itemMaxVolume = StateManager.Instance().waterTanks[0]["itemMaxVolume"];
                        StateManager.Instance().waterTanks.RemoveAt(0);
                    }

                    break;
                case FACTORYTYPE.DynamiteMachine:
                    if(StateManager.Instance().dynamiteMachines.Count > 0)
                    {
                        currentItemNum = StateManager.Instance().dynamiteMachines[0]["currentItemNum"];
                        itemMaxVolume = StateManager.Instance().dynamiteMachines[0]["itemMaxVolume"];
                        StateManager.Instance().dynamiteMachines.RemoveAt(0);
                    }

                    break;
            }
        }
    }

    // 엔진이 일정 시간마다 불나는 이벤트
    public void EngineOverheating()
    {
        Debug.Log($":::: {gameObject.name}에 불이 났습니다 ::::");
        StartCoroutine(FactoryInFire());
    }

    IEnumerator FactoryInFire()
    {
        int loopNum = 0;
        isHeating = true;

        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFireTime += Time.deltaTime;

            if (currentFireTime > fireDeadTime)
            {
                Debug.Log("::::: GAME OVER :::::");
                //GameManager.Instance().GameOver();
                isHeating = false;
                currentFireTime = 0;
                break;
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    // PlayerManager.cs에서 hit 태그가 Factory 이며 & 플레이어가 물통을 들고 있다면 실행
    public void FireSuppression()
    {
        StopCoroutine(FactoryInFire());
        isHeating = false;
        currentFireTime = 0;

        object[] data = new object[] { true };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORY_HEATING, data, raiseEventOptions, SendOptions.SendReliable);
    }

    // 아이템 제작할 수 있는지 확인
    public void ItemProductionCheck()
    {
        if (StateManager.Instance().IngredientCheck(ingredient_1, ingredient_2, amount_1, amount_2) && currentItemNum < itemMaxVolume)
        {
            Debug.Log($":::: {generateItem} 제작 시작 ::::");
            StartCoroutine(ItemProduction());
            //otehrs에서도 실행
            object[] data = new object[] { false };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORY_ACTION, data, raiseEventOptions, SendOptions.SendReliable);
            QuestManager.Instance().UpdateProgress(generateItem, 1);
        }  
    }

    // 아이템 제작 실행
    IEnumerator ItemProduction()
    {
        int loopNum = 0;
        isWorking = true;

        while (true)
        {
            // 아이템 제작 효과 구현

            yield return new WaitForEndOfFrame();
            currentGenerateTime += Time.deltaTime;

            if (currentGenerateTime > generateTime)
            {
                currentGenerateTime = 0;
                isWorking = false;
                break;
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }

        // 아이템 제작 완료
        Debug.Log($"{gameObject.name} Generate ::: " + generateItem);
        ItemAdd();
        ItemProductionCheck();
    }

    // Machine에서 아이템 생성 시 저장 개수 증가
    public void ItemAdd()
    {
        currentItemNum++;
        QuestManager.Instance().UpdateProgress(generateItem, 1);
    }

    // Machine의 아이템 사용 → Player.cs에서 Machien 내의 아이템을 가져가려 할 때 실행 
    public bool ItemUse()
    {
        if(currentItemNum <= 0 /*&& 1==2*/)
        {
            Debug.Log($"{gameObject.name} 아이템이 없습니다....");
            return false;
        }
        else
        {
            return true;
        }
    }

    // 아이템 생성 → 플레이어 손에 생성
    public GameObject ItemGenerate()
    {
        //이구간 others에서도 실행
        currentItemNum--;
        object[] data = new object[] { true };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORY_ACTION, data, raiseEventOptions, SendOptions.SendReliable);
        return PhotonNetwork.Instantiate(generateItem,new Vector3(0,0,0),Quaternion.identity);
    }

    // TODO : 이유정 2024.01.15 FactoryManager.cs FactoryJsonLoad(string path)
    void FactoryJsonLoad(string _path)
    {
        TextAsset json = (TextAsset)Resources.Load(_path);
        string jsonStr = json.text;

        var jsonData = JSON.Parse(jsonStr);

        for(int i = 0; i < jsonData.Count; i++)
        {
            if (jsonData[i]["TYPE"].Equals(factoryType.ToString()))
            {
                ingredient_1 = jsonData[i]["INGREDIENT_1"];
                amount_1 = (int)jsonData[i]["AMOUNT_1"];
                ingredient_2 = jsonData[i]["INGREDIENT_2"];
                amount_2 = (int)jsonData[i]["AMOUNT_2"];
                generateTime = (int)jsonData[i]["GENERATE_TIME"];
                generateItem = jsonData[i]["GENERATE"];

                break;
            }
        }
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.FACTORY_ACTION)
        {
            // 다른 플레이어들이 호출한 RPC로 미터 값을 받음
            object[] receivedData = (object[])photonEvent.CustomData;
            bool isMinusCurrentItem = (bool)receivedData[0];
            if (isMinusCurrentItem)
            {
                currentItemNum--;
            }
            else
            {
                StartCoroutine(ItemProduction());
            }
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.FACTORY_HEATING)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            bool isHeating = (bool)receivedData[0];
            if (isHeating)
            {
                StopCoroutine(FactoryInFire());
                isHeating = false;
                currentFireTime = 0;
            }
            else
            {
                EngineOverheating();
            }
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
