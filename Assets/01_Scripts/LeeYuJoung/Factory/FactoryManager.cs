using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System;

public class FactoryManager : MonoBehaviour
{
    public enum FACTORYTYPE
    {
        ENGINE,
        STORAGE,
        MACHINE
    }
    public FACTORYTYPE factoryType;

    private FactoryController factoryController;
    public Dictionary<string, int> storages = new Dictionary<string, int>();

    public string dataPath;
    public int machineType;

    public string ingredient_1;
    public int amount_1;
    public string ingredient_2;
    public int amount_2;
    public float generateTime;
    public string generateItem;

    public const int maxVolume = 5;

    void Start()
    {
        if (factoryType == FACTORYTYPE.MACHINE)
            FactoryJsonLoad(dataPath);

        factoryController = GetComponent<FactoryController>();
    }

    // ::::: UI 확인용 버튼 :::::
    public void OnWoodAddButton()
    {
        IngredientAdd("Wood", 1);
    }

    // ::::: UI 확인용 버튼 :::::
    public void OnSteelAddButton()
    {
        IngredientAdd("Steel", 1);
    }

    // Storage 내에 자원 저장
    public void IngredientAdd(string _ingredient, int _amount)
    {
        if (!storages.ContainsKey(_ingredient))
        {
            storages.Add(_ingredient, 0);
        }

        storages[_ingredient] += _amount;
        GetAllMachine();

        Debug.Log($":::: 저장소에 재료를 저장 :::: {_ingredient} :: {storages[_ingredient]}");
    }

    // Storage 내의 자원 사용
    public void IngredientUse(string _ingredient, int _amount)
    {
        if (!storages.ContainsKey(_ingredient))
        {
            storages.Add(_ingredient, 0);
        }

        if (storages[_ingredient] <= 0)
            return;

        storages[_ingredient] -= _amount;
    }

    // Machine에서 아이템 생성 시 저장 개수 증가
    public void MachineStorageAdd()
    {
        if (!storages.ContainsKey(generateItem))
        {
            storages.Add(generateItem, 0);
        }

        storages[generateItem] += 1;
    }

    // Machine의 아이템 사용 → Player.cs에서 아이템을 가져가려 할 때 실행 
    public void MachineStorageUse()
    {
        if (!storages.ContainsKey(generateItem))
        {
            storages.Add(generateItem, 0);
        }

        if (storages[generateItem] <= 0)
            return;

        // 아이템 플레이어 손에 생성 메서드 실행
        factoryController.ItemGenerate(generateItem);
        storages[generateItem] -= 1;
    }

    public void OnIngredientCheck()
    {
        if (!storages.ContainsKey(generateItem))
        {
            storages.Add(generateItem, 0);
        }

        factoryController.IngredientCheck(ingredient_1, ingredient_2, amount_1, amount_2);
    }

    public void GetAllMachine()
    {
        Debug.Log(":::: 확인 시작 ::::");
        GameObject[] _machines = GameObject.FindGameObjectsWithTag("Factory");

        for (int i = 0; i < _machines.Length; i++)
        {
            FactoryManager _fm = _machines[i].GetComponent<FactoryManager>();
            Debug.Log($"{_fm.factoryType}");

            if (_fm.factoryType == FACTORYTYPE.STORAGE)
                return;

            if (_fm.storages[_fm.generateItem] >= maxVolume)
            {
                Debug.Log(":::: 용량이 다 찼습니다 ::::");
                return;
            }

            _fm.OnIngredientCheck();
        }
    }

    // TODO : 이유정 2024.01.15 FactoryManager.cs FactoryJsonLoad(string path)
    void FactoryJsonLoad(string _path)
    {
        TextAsset json = (TextAsset)Resources.Load(_path);
        string jsonStr = json.text;

        var jsonData = JSON.Parse(jsonStr);

        ingredient_1 = jsonData[machineType]["INGREDIENT_1"];
        amount_1 = (int)jsonData[machineType]["AMOUNT_1"];
        ingredient_2 = jsonData[machineType]["INGREDIENT_2"];
        amount_2 = (int)jsonData[machineType]["AMOUNT_2"];
        generateTime = (int)jsonData[machineType]["GENERATE_TIME"];
        generateItem = jsonData[machineType]["GENERATE"];
    }
}
