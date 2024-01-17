using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int engineUpgradePrice = 3;
    public int storageUpgradePrice = 2;
    public int machineUpgradePrice = 1;
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
    public void UpgradeMachine(GameObject _gameObject)
    {
        if (StateManager.Instance().voltNum >= machineUpgradePrice)
        {
            // ���ۼ� �� �뷮 �� ����
            FactoryManager _fm = _gameObject.GetComponent<FactoryManager>();

            if(_fm != null)
            {
                _fm.itemMaxVolume += 2;
                storageUpgradePrice += 1;
            }
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ��� �߰� ����
    public void BuyMachine(GameObject _gameObject)
    {
        if(StateManager.Instance().voltNum >= machineAddPrice)
        {
            // �÷��̾ ���� ��� List�� �߰�
            StateManager.Instance().factorys.Add(_gameObject);
            machineAddPrice += 1;
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }
}
