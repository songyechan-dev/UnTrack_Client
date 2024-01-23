using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeeYuJoung;

public class UpgradeManager : MonoBehaviour
{
    public int engineUpgradePrice = 3;
    public int storageUpgradePrice = 2;
    public Dictionary<string, int[]> factoryPrice = new Dictionary<string, int[]>() 
    { { "ProductionMachine", new int[] { 1 } }, { "WaterTank", new int[] { 1 } }, { "DynamiteMachine", new int[] { 1 } } };
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
    public void UpgradeMachine(FactoryManager.FACTORYTYPE _factoryType, int _idx)
    {
        if (StateManager.Instance().voltNum >= factoryPrice[_factoryType.ToString()][_idx])
        {
            // 제작소 총 용량 증가
            if (StateManager.Instance().factorys[_factoryType.ToString()].Count != 0)
            {
                Debug.Log($"::: 업그레이드 시작 ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}");
                StateManager.Instance().factorys[_factoryType.ToString()][_idx][1] += 2;
                factoryPrice[_factoryType.ToString()][_idx] += 1;
                Debug.Log($"::: 업그레이드 성공 ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}"); ;
            }
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 기계 추가 구매
    public void BuyMachine(FactoryManager.FACTORYTYPE _factoryType)
    {
        if(StateManager.Instance().voltNum >= machineAddPrice)
        {
            // 플레이어가 가진 기계 List에 추가
            StateManager.Instance().factorys[_factoryType.ToString()].Add(new int[] { 0, 5 });
            machineAddPrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // UI 확인용 
    public void NextRound()
    {
        StateManager.Instance().BringFactoryValue();
    }

    public void Production1Upgrade()
    {
        UpgradeMachine(FactoryManager.FACTORYTYPE.ProductionMachine, 0);
    }

    public void WaterTankUpgrade()
    {
        UpgradeMachine(FactoryManager.FACTORYTYPE.WaterTank, 0);
    }

    public void DynamiteUpgrade()
    {
        UpgradeMachine(FactoryManager.FACTORYTYPE.DynamiteMachine, 0);
    }
}
