using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int engineUpgradePrice = 3;
    public int storageUpgradePrice = 2;
    public int machineUpgradePrice = 1;
    public int machineAddPrice = 2;

    // 엔진 업그레이드
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= engineUpgradePrice)
        {
            // 엔진 총 용량 1개 증가
            StateManager.Instance().engineMaxVolume += 1;
            engineUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 저장소 업그레이드
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= storageUpgradePrice)
        {
            // 저장소 총 용량 5개 증가
            StateManager.Instance().storageMaxVolume += 5;
            storageUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 기계 업그레이드 → 업그레이드하고 싶은 기계 선택
    public void UpgradeMachine(GameObject _gameObject)
    {
        if (StateManager.Instance().voltNum >= machineUpgradePrice)
        {
            // 제작소 총 용량 개 증가
            FactoryManager _fm = _gameObject.GetComponent<FactoryManager>();

            if(_fm != null)
            {
                _fm.itemMaxVolume += 2;
                storageUpgradePrice += 1;
            }
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 기계 추가 구매
    public void BuyMachine(GameObject _gameObject)
    {
        if(StateManager.Instance().voltNum >= machineAddPrice)
        {
            // 플레이어가 가진 기계 List에 추가
            StateManager.Instance().factorys.Add(_gameObject);
            machineAddPrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }
}
