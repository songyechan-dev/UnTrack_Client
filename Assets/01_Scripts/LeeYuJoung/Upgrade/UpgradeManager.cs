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

    // ���� ���׷��̵�
    public void UpgradeEngine()
    {
        if(StateManager.Instance().voltNum >= engineUpgradePrice)
        {

        }
        else
        {
            Debug.Log(":::: ��Ʈ�� ���� ���׷��̵� ���� ::::");
        }
    }

    // ����� ���׷��̵�
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= storageUpgradePrice)
        {

        }
        else
        {
            Debug.Log(":::: ��Ʈ�� ���� ���׷��̵� ���� ::::");
        }
    }

    // ��� ���׷��̵� �� ���׷��̵��ϰ� ���� ��� ����
    public void UpgradeMachine(GameObject _gameObject)
    {

    }

    // ��� �߰� ����
    public void BuyMachine(GameObject _gameObject)
    {

    }
}
