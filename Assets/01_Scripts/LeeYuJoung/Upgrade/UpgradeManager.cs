using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeeYuJoung;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class UpgradeManager : MonoBehaviour
{
    public int engineUpgradePrice = 3;
    public int storageUpgradePrice = 2;

    public Dictionary<string, List<int>> factoryPrice = new Dictionary<string, List<int>>()
    { { "ProductionMachine", new List<int> { 1 } }, { "WaterTank", new List<int> { 1 } }, { "DynamiteMachine", new List<int> { 1 } } };
    public Dictionary<string, int> machineAddPrice = new Dictionary<string, int>() 
    { { "ProductionMachine", 2 }, { "WaterTank", 2 }, { "DynamiteMachine", 2 } };

    // ���� ���׷��̵�
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= engineUpgradePrice)
        {
            Debug.Log(":::: ���� ���׷��̵� ���� ::::");
            StateManager.Instance().engineMaxVolume += 1;
            StateManager.Instance().voltNum -= engineUpgradePrice;
            engineUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ����� ���׷��̵�
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= storageUpgradePrice)
        {
            Debug.Log(":::: ����� ���׷��̵� ���� ::::");
            StateManager.Instance().storageMaxVolume += 5;
            StateManager.Instance().voltNum -= storageUpgradePrice;
            storageUpgradePrice += 1;
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ��� ���׷��̵� �� ���׷��̵��ϰ� ���� ��� ����
    public void UpgradeMachine(FactoryManager.FACTORYTYPE _factoryType, int _idx)
    {
        if (StateManager.Instance().voltNum >= factoryPrice[_factoryType.ToString()][_idx])
        {
            if (StateManager.Instance().factorys[_factoryType.ToString()].Count != 0)
            {
                Debug.Log($"::: ���׷��̵� ���� ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}");
                StateManager.Instance().factorys[_factoryType.ToString()][_idx][1] += 2;
                StateManager.Instance().voltNum -= factoryPrice[_factoryType.ToString()][_idx];
                factoryPrice[_factoryType.ToString()][_idx] += 1;
            }
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ��� �߰� ����
    public void BuyMachine(FactoryManager.FACTORYTYPE _factoryType)
    {
        if(StateManager.Instance().voltNum >= machineAddPrice[_factoryType.ToString()])
        {
            Debug.Log($":::: {_factoryType} ���� �Ϸ� ::::");
            Debug.Log(StateManager.Instance().factorys[_factoryType.ToString()].Count);

            StateManager.Instance().factorys[_factoryType.ToString()].Add(new int[] { 0, 5 });
            StateManager.Instance().voltNum -= machineAddPrice[_factoryType.ToString()];
            machineAddPrice[_factoryType.ToString()] += 1;
            factoryPrice[_factoryType.ToString()].Add(1);

            Debug.Log(StateManager.Instance().factorys[_factoryType.ToString()].Count);
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���� ���� ::::");
        }
    }

    public void ShowUpgradeMachine(Transform[] _pos)
    {
        int _idx = 0;
        StateManager.Instance().BringFactoryValue();

        for (int i = 0; i < StateManager.Instance().productionMachines.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeProductionMachine"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo_LeeYuJoung>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = factoryPrice["ProductionMachine"][i].ToString();
        }

        for (int i = 0; i < StateManager.Instance().waterTanks.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeWaterTank"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo_LeeYuJoung>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = factoryPrice["WaterTank"][i].ToString();
        }

        for (int i = 0; i < StateManager.Instance().dynamiteMachines.Count; i++)
        {
            GameObject _machine = Instantiate((GameObject)Resources.Load("UpgradeMachine/UpgradeDynamiteMachine"), _pos[_idx++]);
            _machine.GetComponentInChildren<PlayableButtonInfo_LeeYuJoung>().machineUpgradeIDX = i;
            _machine.transform.GetChild(1).GetComponent<TextMesh>().text = factoryPrice["DynamiteMachine"][i].ToString();
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
