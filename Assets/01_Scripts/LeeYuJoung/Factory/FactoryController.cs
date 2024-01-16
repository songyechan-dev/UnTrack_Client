using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    private FactoryManager factoryManager;
    private FactoryManager storageFactoryManager;
    public float currentTime = 0;
    public bool isWorking = false;

    private void Start()
    {
        factoryManager = GetComponent<FactoryManager>();
        storageFactoryManager = GameObject.Find("Storage").GetComponent<FactoryManager>();
    }

    // 엔진이 일정 시간마다 불나는 이벤트
    public void EngineOverheating()
    {

    }

    // Storage에 재료가 충분한지 확인 
    // → 플레이어가 저장소에 재료 넣을 때 & 아이템 제작을 끝냈을 때 확인
    public void IngredientCheck(string _ingredient1, string _ingredient2, int _amount1, int _amount2)
    {
        if (_ingredient1.Equals(_ingredient2))
        {
            if (storageFactoryManager.storages[_ingredient1] >= _amount1 + _amount2)
            {
                Debug.Log(":::: 제작 시작 ::::");
                storageFactoryManager.IngredientUse(_ingredient1, _amount1 + _amount2);
            }
            else
            {
                Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
            }
        }
        else
        {
            if (storageFactoryManager.storages[_ingredient1] >= _amount1 && storageFactoryManager.storages[_ingredient2] >= _amount2)
            {
                Debug.Log(":::: 제작 시작 ::::");
                storageFactoryManager.IngredientUse(_ingredient1, _amount1);
                storageFactoryManager.IngredientUse(_ingredient2, _amount2);
            }
            else
            {
                Debug.Log(":::: 재료가 부족하여 아이템을 생성할 수 없습니다 ::::");
            }
        }
    }

    // 아이템 제작 실행
    IEnumerator ItemProduction(float _generateTime, string _generateItem)
    {
        int loopNum = 0;

        while (true)
        {
            // 아이템 제작 효과 구현

            yield return new WaitForEndOfFrame();
            Debug.Log("CurrentTime ::: " + currentTime);
            currentTime += Time.deltaTime;

            if (currentTime > _generateTime)
            {
                // 아이템 제작 완료
                Debug.Log("Generate ::: " + _generateItem);
                currentTime = 0;
            }

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    // 아이템 생성
    public void ItemGenerate(string _generateItem)
    {
        GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/SongYeChan/{_generateItem}.prefab", typeof(GameObject)) as GameObject;
        GameObject _object = Instantiate(_item, transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.rotation);
        _object.name = _generateItem;
    }
}
