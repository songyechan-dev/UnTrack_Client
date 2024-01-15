using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

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
    }

    // 엔진 일정 시간마다 불나는 이벤트
    public void EngineOverheating()
    {

    }

    // Storage에 플레이어가 재료를 저장할 때 효과 구현 및 Inevntory 저장 함수 실행
    // → Player가 Space 누렀다면 & FACTORYTYPE == STORAGE라면 실행
    public void IngredientSave()
    {

    }

    // Storage에 재료가 충분한지 확인 후 아이템 제작
    public void ItemProduction()
    {
        
    }

    //TODO : 이유정 2024.01.15 FactoryManager.cs FactoryJsonLoad(string path)
    void FactoryJsonLoad(string path)
    {
        TextAsset json = (TextAsset)Resources.Load(path);
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
