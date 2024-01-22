using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LeeYuJoung
{
    public class StateManager : MonoBehaviour
    {
        public List<Dictionary<string, int>> productionMachines = new List<Dictionary<string, int>>();
        public List<Dictionary<string, int>> waterTanks = new List<Dictionary<string, int>>();
        public List<Dictionary<string, int>> dynamiteMachines = new List<Dictionary<string, int>>();

        private static StateManager instance;
        public static StateManager Instance()
        {
            return instance;
        }

        public int engineMaxVolume = 5;      // 엔진 최대 수용 가능한 기계 개수
        public int engineCurrentVolume = 3;  // 현재 엔진이 수용한 기계 개수 ( 기본 시작 : 엔진 1개, 저장소 1개, 제작소 1개 )

        public Dictionary<string, List<int[]>> factorys = new Dictionary<string, List<int[]>>()
        { { "ProductionMachine", new List<int[]> { new int[] { 0, 5 } } }, { "WaterTank", new List<int[]> { new int[] { 0, 5 } } }, { "DynamiteMachine", new List<int[]>() } };

        public List<GameObject> sceneFactorys = new List<GameObject>();   // 현재 엔진이 가진 제작소들
        public int voltNum = 0;              // 현재 보유 볼트(재화) 개수

        public Dictionary<string, int> storages = new Dictionary<string, int>();  // Storage 재료 저장 공간
        public int storageMaxVolume = 10;  // Storage 저장 용량 (재료 종류 상관 없이 총 합 비교)

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

            BringFactoryValue();
            storages.Add("WOOD", 0);
            storages.Add("STEEL", 0);
            sceneFactorys = GameObject.FindGameObjectsWithTag("Factory").ToList();
        }

        private void Update()
        {

        }

        // :::::: UI 확인용 버튼 나중에 삭제 ::::::
        public void OnUIExample(int _num)
        {
            storageText.text = $"STORAGE TOTALVOLUME {_num} / 10";
            woodText.text = $"Wood : {storages["WOOD"]}";
            steelText.text = $"Steel : {storages["STEEL"]}";
        }

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

        // 플레이어가 현재 들고 있는 오브젝트 정보 매개변수로 전달 받기(해당 오브젝트의 이름)
        // → 재료는 저장소에 내려 놓기 직전에 해당 함수 실행하기
        // → 트랙은 트랙 옆에 내려 놓기 직전에 해당 함수 실행하기
        public void CheckAccumulateCount(string _name, int _amount)
        {
            int currentNum = (int)GetType().GetField($"accumulate{_name}").GetValue(instance);
            GetType().GetField($"accumulate{_name}").SetValue(instance, currentNum + _amount);

            // Quest 진행도 업그레이드
            QuestManager.Instance().UpdateProgress(_name, _amount);
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

                for (int i = 0; i < sceneFactorys.Count; i++)
                {
                    FactoryManager _fm = sceneFactorys[i].GetComponent<FactoryManager>();

                    if (!_fm.isWorking)
                    {
                        _fm.ItemProductionCheck();
                    }
                }

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

            OnUIExample(storages["WOOD"] + storages["STEEL"]);
            Debug.Log($":::: 저장소 재료 사용 :::: {_ingredient} :: {storages[_ingredient]}");
        }

        // Storage에 아이템을 제작할 재료가 충분한지 확인 
        // → 플레이어가 저장소에 재료 넣을 때 & 아이템 제작을 끝냈을 때 확인
        public bool IngredientCheck(string _ingredient1, string _ingredient2, int _amount1, int _amount2)
        {
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
    }
}