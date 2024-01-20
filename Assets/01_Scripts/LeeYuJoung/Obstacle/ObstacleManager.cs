using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System;

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

    void Start()
    {
        ObstacleJsonLoad(dataPath);
    }

    // 현재 Obstacle의 작업 가능 여부 확인
    public void ObstacleWorking(string _equipment)
    {
        if (isWorking || !equipmentType.Equals(_equipment))
        {
            Debug.Log("::: 이미 작업 중 이거나 장비가 맞지 않음 :::");
            return;
        }

        isWorking = true;
        StartCoroutine(ObstacleDelete());
    }

    // Obstacle 제거
    IEnumerator ObstacleDelete()
    {
        // 점점 작아지는 효과 구현하기


        int loopNum = 0;

        while (isWorking)
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("CurrentTime ::: " + currentTime);
            currentTime += Time.deltaTime;

            if (currentTime > workTime)
            {
                currentTime = 0;
                isWorking = false;

                GenerateIngredient();
                Destroy(gameObject);
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    // 재료 생성
    public void GenerateIngredient()
    {
        Debug.Log("Generate ::: " + generateItem);
        GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/LeeYouJoung/Item/{generateItem}.prefab", typeof(GameObject)) as GameObject;
        GameObject _object =  Instantiate(_item, transform.position, transform.rotation);
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
}
