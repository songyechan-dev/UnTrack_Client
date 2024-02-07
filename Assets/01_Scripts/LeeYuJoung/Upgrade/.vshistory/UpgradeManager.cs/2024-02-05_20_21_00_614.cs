using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeeYuJoung;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class UpgradeManager : MonoBehaviour
{
    // 엔진 업그레이드
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= StateManager.Instance().engineUpgradePrice)
        {
            Debug.Log(":::: 엔진 업그레이드 성공 ::::");
            StateManager.Instance().engineMaxVolume += 1;
            StateManager.Instance().SetVolt(false, StateManager.Instance().engineUpgradePrice);
            StateManager.Instance().engineUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 저장소 업그레이드
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= StateManager.Instance().storageUpgradePrice)
        {
            Debug.Log(":::: 저장소 업그레이드 성공 ::::");
            StateManager.Instance().storageMaxVolume += 5;
            StateManager.Instance().SetVolt(false,StateManager.Instance().storageUpgradePrice);
            StateManager.Instance().storageUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: 볼트 부족 업그레이드 실패 ::::");
        }
    }

    // 기계 업그레이드 → 업그레이드하고 싶은 기계 선택
    public void UpgradeMachine(FactoryManager.FACTORYTYPE _factoryType, int _idx)
    {
        if (StateManager.Instance().voltNum >= StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx])
        {
            if (StateManager.Instance().factorys[_factoryType.ToString()].Count != 0)
            {
                Debug.Log($"::: 업그레이드 성공 ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}");
                StateManager.Instance().factorys[_factoryType.ToString()][_idx][1] += 2;
                StateManager.Instance().SetVolt(false, StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx]);
                StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx] += 1;
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
        if(StateManager.Instance().voltNum >= StateManager.Instance().machineAddPrice[_factoryType.ToString()] && 9 >StateManager.Instance().productionMachines.Count + StateManager.Instance().dynamiteMachines.Count + StateManager.Instance().waterTanks.Count)
        {
            Debug.Log($":::: {_factoryType} 구매 완료 ::::");
            Debug.Log(StateManager.Instance().factorys[_factoryType.ToString()].Count);

            StateManager.Instance().factorys[_factoryType.ToString()].Add(new int[] { 0, 5 });
            StateManager.Instance().SetVolt(false, StateManager.Instance().machineAddPrice[_factoryType.ToString()]);
            StateManager.Instance().machineAddPrice[_factoryType.ToString()] += 1;
            StateManager.Instance().factoryPrice[_factoryType.ToString()].Add(1);

            Debug.Log(StateManager.Instance().factorys[_factoryType.ToString()].Count);
        }
        else
        {
            Debug.Log(":::: 볼트 부족 구매 실패 ::::");
        }
    }

    public void ShowUpgradeMachine(Transform[] _pos)
    {
        int _idx = 0;
        StateManager.Instance().BringFactoryValue();

        for (int i = 0; i < StateManager.Instance().productionMachines.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeProductionMachine"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["ProductionMachine"][i].ToString();
        }

        for (int i = 0; i < StateManager.Instance().waterTanks.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeWaterTank"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["WaterTank"][i].ToString();
        }

        for (int i = 0; i < StateManager.Instance().dynamiteMachines.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeDynamiteMachine"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = StateManager.Instance().factoryPrice["DynamiteMachine"][i].ToString();
        }
    }

    public void ClearUpgradeMachine(Transform[] _pos)
    {
        for (int i = 0; i < _pos.Length; i++)
        {
            if (_pos[i].childCount > 0)
            {
                Destroy(_pos[i].GetChild(0).gameObject);
            }
        }

        StateManager.Instance().productionMachines.Clear();
        StateManager.Instance().dynamiteMachines.Clear();
        StateManager.Instance().waterTanks.Clear();
    }
}
