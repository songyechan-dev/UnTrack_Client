using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int engineUpgradePrice;
    public int storageUpgradePrice;
    public int machineUpgradePrice;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 엔진 업그레이드
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= engineUpgradePrice)
        {

        }
        else
        {
            Debug.Log(":::: 볼트가 부족 업그레이드 실패 ::::");
        }
    }

    // 저장소 업그레이드
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= storageUpgradePrice)
        {

        }
        else
        {
            Debug.Log(":::: 볼트가 부족 업그레이드 실패 ::::");
        }
    }

    // 기계 업그레이드 → 업그레이드하고 싶은 기계 선택
    public void UpgradeMachine(GameObject _gameObject)
    {

    }

    // 기계 추가 구매
    public void BuyMachine(GameObject _gameObject)
    {

    }
}
