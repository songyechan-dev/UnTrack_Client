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

    public string dataPath;
    public int machineType;

    private string ingredient_1;
    private int amount_1;
    private string ingredient_2;
    private int amount_2;
    private float generateTime;
    private string generateItem;

    public float currentTime = 0;

    void Start()
    {
        if (factoryType == FACTORYTYPE.MACHINE)
            FactoryJsonLoad(dataPath);

        factoryController = GetComponent<FactoryController>();

        // 재료 확인 작업 
        IngredientCheck();
    }

    // 엔진 일정 시간마다 불나는 이벤트
    public void EngineOverheating()
    {

    }

    // Storage에 플레이어가 재료를 저장할 때 효과 구현 및 Inevntory.cs 저장 함수 실행
    public void IngredientSave(string _ingredient, int _amount)
    {
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().UseInventory(_ingredient, _amount);

        // 재료 저장 효과 구현

    }

    // Storage에 재료가 충분한지 확인
    public void IngredientCheck()
    {
        if (ingredient_1.Equals(ingredient_2))
        {
            if (GameObject.Find("InventoryManager").GetComponent<InventoryManager>().playerStorage[ingredient_1] >= amount_1 + amount_2)
            {
                StartCoroutine(ItemProduction());
                IngredientSave(ingredient_1, amount_1 + amount_2);
            }
            else
            {
                Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
            }
        }
        else
        {
            if (GameObject.Find("InventoryManager").GetComponent<InventoryManager>().playerStorage[ingredient_1] >= amount_1)
            {
                if (GameObject.Find("InventoryManager").GetComponent<InventoryManager>().playerStorage[ingredient_2] >= amount_2)
                {
                    StartCoroutine(ItemProduction());
                    IngredientSave(ingredient_1, amount_1);
                    IngredientSave(ingredient_2, amount_2);
                }
                else
                {
                    Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
                }
            }
            else
            {
                Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
            }
        }
    }

    // 아이템 제작 실행
    IEnumerator ItemProduction()
    {
        int loopNum = 0;

        while (true)
        {
            // 아이템 제작 효과 구현

            yield return new WaitForEndOfFrame();
            Debug.Log("CurrentTime ::: " + currentTime);
            currentTime += Time.deltaTime;

            if (currentTime > generateTime)
            {
                // 아이템 제작 완료
                Debug.Log("Generate ::: " + generateItem);
                currentTime = 0;
                GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/SongYeChan/{generateItem}.prefab", typeof(GameObject)) as GameObject;
                GameObject _object = Instantiate(_item, transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.rotation);
                _object.name = generateItem;
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
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
