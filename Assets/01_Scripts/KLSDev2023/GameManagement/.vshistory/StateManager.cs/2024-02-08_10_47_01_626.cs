using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LeeYuJoung
{
    public class StateManager : MonoBehaviourPun
    {
        private static StateManager instance;
        public static StateManager Instance()
        {
            return instance;
        }

        public int voltNum = 0;              // 현재 보유 볼트(재화) 개수
        public int engineMaxVolume = 5;      // 엔진 최대 수용 가능한 기계 개수
        public int engineCurrentVolume = 3;  // 현재 엔진이 수용한 기계 개수 ( 기본 시작 : 엔진 1개, 저장소 1개, 제작소 1개 )

        public Dictionary<string, int> storages = new Dictionary<string, int>();  // Storage 재료 저장 공간
        public int storageMaxVolume = 10;  // Storage 저장 용량 (재료 종류 상관 없이 총 합 비교)

        public SerializedDictionary<string, List<int[]>> factorys = new SerializedDictionary<string, List<int[]>>()
        { { "ProductionMachine", new List<int[]> { new int[] { 0, 5 } } }, { "WaterTank", new List<int[]> { new int[] { 0, 5 } } }, { "DynamiteMachine", new List<int[]>() } };

        public List<Dictionary<string, int>> productionMachines = new List<Dictionary<string, int>>();
        public List<Dictionary<string, int>> waterTanks = new List<Dictionary<string, int>>();
        public List<Dictionary<string, int>> dynamiteMachines = new List<Dictionary<string, int>>();

        public List<GameObject> sceneFactorys = new List<GameObject>();   // 현재 엔진이 가진 제작소들

        public int engineUpgradePrice = 3;
        public int storageUpgradePrice = 2;

        public Dictionary<string, List<int>> factoryPrice = new Dictionary<string, List<int>>()
        { { "ProductionMachine", new List<int> { 1 } }, { "WaterTank", new List<int> { 1 } }, { "DynamiteMachine", new List<int> { 1 } } };
        public Dictionary<string, int> machineAddPrice = new Dictionary<string, int>()
        { { "ProductionMachine", 2 }, { "WaterTank", 2 }, { "DynamiteMachine", 2 } };

        // TODO : 이유정 2024.01.23 StateManager.cs → TimeManager.cs 이동
        public float currentTime = 0;
        public float fireTime = 20.0f;

        public Text storageText;
        public Text woodText;
        public Text steelText;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            storages.Add("WOOD", 0);
            storages.Add("STEEL", 0);
        }



        private void Update()
        {
            if (PhotonNetwork.IsMasterClient && GameManager.Instance().gameMode.Equals(GameManager.GameMode.Play) && GameManager.Instance().gameState.Equals(GameManager.GameState.GameStart))
            {
                currentTime += Time.deltaTime;

                if (currentTime > fireTime)
                {
                    Fire();
                }
            }            
        }

        // :::::: UI 확인용 버튼 나중에 삭제 ::::::
        public void OnUIExample(int _num)
        {
            //storageText.text = $"STORAGE TOTALVOLUME {_num} / 10";
            //woodText.text = $"Wood : {storages["WOOD"]}";
            //steelText.text = $"Steel : {storages["STEEL"]}";
        }

        // 업그레이드 끝나고 라운드 시작하기 전 마다 실행
        public void BringFactoryValue()
        {
            for(int i = 0; i < factorys["ProductionMachine"].Count; i++)
            {
                productionMachines.Add(new Dictionary<string, int> { { "currentItemNum", factorys["ProductionMachine"][i][0] }, { "itemMaxVolume", factorys["ProductionMachine"][i][1] } });
            }

            for (int i = 0; i < factorys["WaterTank"].Count; i++)
            {
                waterTanks.Add(new Dictionary<string, int> { { "currentItemNum", factorys["WaterTank"][i][0] }, { "itemMaxVolume", factorys["WaterTank"][i][1] } });
            }

            for (int i = 0; i < factorys["DynamiteMachine"].Count; i++)
            {
                dynamiteMachines.Add(new Dictionary<string, int> { { "currentItemNum", factorys["DynamiteMachine"][i][0] }, { "itemMaxVolume", factorys["DynamiteMachine"][i][1] } });
            }
        }

        //// 플레이어가 현재 들고 있는 오브젝트 정보 매개변수로 전달 받기(해당 오브젝트의 이름)
        //public void CheckAccumulateCount(string _name, int _amount)
        //{
        //    int currentNum = (int)GetType().GetField($"accumulate{_name}").GetValue(instance);
        //    GetType().GetField($"accumulate{_name}").SetValue(instance, currentNum + _amount);

        //    // Quest 진행도 업그레이드
        //    QuestManager.Instance().UpdateProgress(_name, _amount);
        //}

        public void Fire()
        {
            int _idx = Random.Range(0, sceneFactorys.Count);

            if (_idx < sceneFactorys.Count && sceneFactorys[_idx] != null)
            {
                FactoryManager factoryManager = sceneFactorys[_idx].GetComponent<FactoryManager>();

                if (factoryManager != null)
                {
                    if (!factoryManager.isHeating && PhotonNetwork.IsMasterClient)
                    {
                        factoryManager.EngineOverheating();
                        currentTime = 0;
                        object[] data = new object[] { false,factoryManager.GetComponent<PhotonView>().ViewID };
                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORY_HEATING, data, raiseEventOptions, SendOptions.SendReliable);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }

        // Storage 내에 재료 저장
        public bool IngredientAdd(string _ingredient, int _amount)
        {
            if (!storages.ContainsKey(_ingredient))
            {
                storages.Add(_ingredient, 0);
            }
            List<string> _keys = new List<string>(storages.Keys);
            int storageTotalNum = 0;

            for (int i = 0; i < _keys.Count; i++)
            {
                storageTotalNum += storages[_keys[i]];
            }

            if (storageTotalNum + _amount > storageMaxVolume)
            {
                Debug.Log($":::: 저장소 자리가 가득 찼습니다 ::::");
                return false;
            }
            else
            {
                storages[_ingredient] += _amount;
                Debug.Log("아이템 추가 :::::" + storages[_ingredient]);
                for (int i = 0; i < sceneFactorys.Count; i++)
                {
                    FactoryManager _fm = sceneFactorys[i].GetComponent<FactoryManager>();

                    if (!_fm.isWorking)
                    {
                        _fm.ItemProductionCheck();
                    }
                }
                object[] data = new object[] { _ingredient,_amount,true };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.ITEM_ADD_STORAGEE, data, raiseEventOptions, SendOptions.SendReliable);
                OnUIExample(storages["WOOD"] + storages["STEEL"]);
                return true;
            }
        }

        // Storage 내의 재료 사용
        public void IngredientUse(string _ingredient, int _amount)
        {
            if (!storages.ContainsKey(_ingredient))
            {
                storages.Add(_ingredient, 0);
            }

            if (storages[_ingredient] <= 0)
            {
                Debug.Log($":::: 저장소에 재료가 부족 ::::");
                return;
            }

            storages[_ingredient] -= _amount;
            object[] data = new object[] { _ingredient, _amount, false };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.ITEM_ADD_STORAGEE, data, raiseEventOptions, SendOptions.SendReliable);
            OnUIExample(storages["WOOD"] + storages["STEEL"]);
            Debug.Log($":::: 저장소 재료 사용 :::: {_ingredient} :: {storages[_ingredient]}");
        }

        // Storage에 아이템을 제작할 재료가 충분한지 확인 
        public bool IngredientCheck(string _ingredient1, string _ingredient2, int _amount1, int _amount2)
        {
            Debug.Log("First ::::" + _ingredient1);
            Debug.Log("Secound ::::" + _ingredient2);
            if (_ingredient1.Equals(_ingredient2))
            {
                if (storages[_ingredient1] >= _amount1 + _amount2)
                {
                    IngredientUse(_ingredient1, _amount1 + _amount2);
                    return true;
                }
                else
                {
                    Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
                    return false;
                }
            }
            else
            {
                if (storages[_ingredient1] >= _amount1 && storages[_ingredient2] >= _amount2)
                {
                    IngredientUse(_ingredient1, _amount1);
                    IngredientUse(_ingredient2, _amount2);
                    return true;
                }
                else
                {
                    Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
                    return false;
                }
            }
        }

        void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == (int)SendDataInfo.Info.ITEM_ADD_STORAGEE)
            {
                object[] receivedData = (object[])photonEvent.CustomData;
                string _ingredient = (string)receivedData[0];
                int _amount = (int)receivedData[1];
                bool isAdd = (bool)receivedData[2];
                if (!storages.ContainsKey(_ingredient))
                {
                    storages.Add(_ingredient, 0);
                }
                if (isAdd)
                {
                    storages[_ingredient] += _amount;
                }
                else
                {
                    storages[_ingredient] -= _amount;
                }
                

                Debug.Log("아이템 추가 :::::" + storages[_ingredient]);
            }
            if (photonEvent.Code == (int)SendDataInfo.Info.FACTORY_HEATING)
            {
                object[] receivedData = (object[])photonEvent.CustomData;
                bool _isFire = (bool)receivedData[0];
                int viewID = (int)receivedData[1];
                GameObject go = PhotonView.Find(viewID).gameObject;
                go.GetComponent<FactoryManager>().EngineOverheating();
            }
            if (photonEvent.Code == (int)SendDataInfo.Info.VOLT_INFO)
            {
                object[] receivedData = (object[])photonEvent.CustomData;
                int _voltCount = (int)receivedData[0];
                if (!PhotonNetwork.IsMasterClient)
                {
                    voltNum = _voltCount;
                }
                if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    UIManager.Instance().volt03.text = voltNum.ToString();
                }
                else if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    UIManager.Instance().voltNumText04.text = voltNum.ToString();
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

        //TODO : VoltSetter 송예찬
        public void SetVolt(bool _isAdd, int num)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (_isAdd)
                {
                    voltNum += num;
                }
                else
                {
                    voltNum -= num;
                }
                if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    UIManager.Instance().volt03.text = voltNum.ToString();
                }
                else if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    UIManager.Instance().voltNumText04.text = voltNum.ToString();
                }
                object[] data = new object[] { voltNum };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.VOLT_INFO, data, raiseEventOptions, SendOptions.SendReliable);
            }
            
        }

    }

}