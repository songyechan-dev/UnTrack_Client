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

    public string dataPath;
    public int machineType;

    public string ingredient_1;
    public int amount_1;
    public string ingredient_2;
    public int amount_2;
    public float generateTime;
    public string generateItem;

    public float heatingDeadTime = 45;
    public float currentHeatingTime = 0;
    public float currentGenerateTime = 0;

    public int currentItemNum = 0;
    public int itemMaxVolume = 5;

    public bool isWorking = false;
    public bool isHeating = false;

    public void Init()
    {
        if (factoryType == FACTORYTYPE.MACHINE)
            FactoryJsonLoad(dataPath);
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
            // 아이템 제작 효과 구현

            yield return new WaitForEndOfFrame();
            currentHeatingTime += Time.deltaTime;

            if (currentHeatingTime > heatingDeadTime)
            {
                // GameOver 처리
                Debug.Log("::::: GAME OVER :::::");
                currentHeatingTime = 0;
                isHeating = false;
                break;
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    // 아이템 제작할 수 있는지 확인
    public void ItemProductionCheck()
    {
        if (StateManager.Instance().IngredientCheck(ingredient_1, ingredient_2, amount_1, amount_2) && currentItemNum < itemMaxVolume)
        {
            Debug.Log($":::: {generateItem} 제작 시작 ::::");
            StartCoroutine(ItemProduction());
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
    }

    // Machine의 아이템 사용 → Player.cs에서 Machien 내의 아이템을 가져가려 할 때 실행 
    public void ItemUse()
    {
        if(currentItemNum <= 0)
        {
            Debug.Log($"{gameObject.name} 아이템이 없습니다....");
            return;
        }

        currentItemNum--;
        ItemGenerate();
    }

    // 아이템 생성 → 플레이어 손에 생성
    public void ItemGenerate()
    {
        GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/SongYeChan/{generateItem}.prefab", typeof(GameObject)) as GameObject;
        GameObject _object = Instantiate(_item, transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.rotation);
        _object.name = generateItem;
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
