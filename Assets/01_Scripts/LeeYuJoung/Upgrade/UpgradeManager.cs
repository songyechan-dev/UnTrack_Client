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

    // ���� ���׷��̵�
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= engineUpgradePrice)
        {
            // ���� �� �뷮 1�� ����
            StateManager.Instance().engineMaxVolume += 1;
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
            // ����� �� �뷮 5�� ����
            StateManager.Instance().storageMaxVolume += 5;
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
            // ���ۼ� �� �뷮 ����
            if (StateManager.Instance().factorys[_factoryType.ToString()].Count != 0)
            {
                Debug.Log($"::: ���׷��̵� ���� ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}");
                StateManager.Instance().factorys[_factoryType.ToString()][_idx][1] += 2;
                factoryPrice[_factoryType.ToString()][_idx] += 1;
                Debug.Log($"::: ���׷��̵� ���� ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}"); ;
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
        if(StateManager.Instance().voltNum >= machineAddPrice)
        {
            // �÷��̾ ���� ��� List�� �߰�
            StateManager.Instance().factorys[_factoryType.ToString()].Add(new int[] { 0, 5 });
            machineAddPrice += 1;
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // UI Ȯ�ο� 
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
