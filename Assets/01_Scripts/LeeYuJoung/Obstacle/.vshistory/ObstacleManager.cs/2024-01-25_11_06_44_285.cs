using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System;
using Photon.Pun;

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
        if (isWorking || !equipmentType.Equals(_equipment))
        {
            Debug.Log("::: 이미 작업 중 이거나 장비가 맞지 않음 :::");
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
        // 점점 작아지는 효과 구현하기


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
                PhotonNetwork.Destroy(gameObject);
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
        GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/LeeYouJoung/Item/{generateItem}.prefab", typeof(GameObject)) as GameObject;
        GameObject _object =  Instantiate(_item, transform.position, transform.rotation);
        _object.transform.position = _player.transform.position;
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
