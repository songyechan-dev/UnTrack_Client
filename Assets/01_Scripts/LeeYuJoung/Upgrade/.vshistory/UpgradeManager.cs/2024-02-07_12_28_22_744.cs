using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeeYuJoung;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Photon.Pun;
using ExitGames.Client.Photon;
using static GameManager;
using Photon.Realtime;

public enum upgradeType
{
    UPGRADE_ENGINE = 0,
    UPGRADE_STORAGE = 1,
    UPGRADE_MACHINE = 2,
    BUY_MACHINE = 3,
}

public class UpgradeManager : MonoBehaviour
{
    // ���� ���׷��̵�
    public void UpgradeEngine()
    {
        if (StateManager.Instance().voltNum >= StateManager.Instance().engineUpgradePrice)
        {
            Debug.Log(":::: ���� ���׷��̵� ���� ::::");
            StateManager.Instance().engineMaxVolume += 1;
            StateManager.Instance().SetVolt(false, StateManager.Instance().engineUpgradePrice);
            StateManager.Instance().engineUpgradePrice += 1;
            object[] data = new object[] { upgradeType.UPGRADE_ENGINE, StateManager.Instance().engineMaxVolume, StateManager.Instance().engineUpgradePrice };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UPGRADE_INFO, data, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ����� ���׷��̵�
    public void UpgradeStorage()
    {
        if (StateManager.Instance().voltNum >= StateManager.Instance().storageUpgradePrice)
        {
            Debug.Log(":::: ����� ���׷��̵� ���� ::::");
            StateManager.Instance().storageMaxVolume += 5;
            StateManager.Instance().SetVolt(false, StateManager.Instance().storageUpgradePrice);
            StateManager.Instance().storageUpgradePrice += 1;
            object[] data = new object[] { upgradeType.UPGRADE_STORAGE, StateManager.Instance().storageMaxVolume, StateManager.Instance().storageUpgradePrice };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UPGRADE_INFO, data, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Debug.Log(":::: ��Ʈ ���� ���׷��̵� ���� ::::");
        }
    }

    // ��� ���׷��̵� �� ���׷��̵��ϰ� ���� ��� ����
    public void UpgradeMachine(FactoryManager.FACTORYTYPE _factoryType, int _idx)
    {
        if (StateManager.Instance().voltNum >= StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx])
        {
            if (StateManager.Instance().factorys[_factoryType.ToString()].Count != 0)
            {
                Debug.Log($"::: ���׷��̵� ���� ::: {StateManager.Instance().factorys[_factoryType.ToString()][_idx][1]}");
                StateManager.Instance().factorys[_factoryType.ToString()][_idx][1] += 2;
                StateManager.Instance().SetVolt(false, StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx]);
                StateManager.Instance().factoryPrice[_factoryType.ToString()][_idx] += 1;
                object[] data = new object[] { upgradeType.UPGRADE_MACHINE, StateManager.Instance().factorys[_factoryType.ToString()][_idx][1], StateManager.Instance().[_factoryType.ToString()][_idx] };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UPGRADE_INFO, data, raiseEventOptions, SendOptions.SendReliable);
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
        if (StateManager.Instance().voltNum >= StateManager.Instance().machineAddPrice[_factoryType.ToString()] && 9 > StateManager.Instance().productionMachines.Count + StateManager.Instance().dynamiteMachines.Count + StateManager.Instance().waterTanks.Count)
        {
            Debug.Log($":::: {_factoryType} ���� �Ϸ� ::::");
            Debug.Log(StateManager.Instance().factorys[_factoryType.ToString()].Count);

            StateManager.Instance().factorys[_factoryType.ToString()].Add(new int[] { 0, 5 });
            StateManager.Instance().SetVolt(false, StateManager.Instance().machineAddPrice[_factoryType.ToString()]);
            StateManager.Instance().machineAddPrice[_factoryType.ToString()] += 1;
            StateManager.Instance().factoryPrice[_factoryType.ToString()].Add(1);



            object[] data = new object[] { upgradeType.BUY_MACHINE, StateManager.Instance().factorys[_factoryType.ToString()][_idx][1], StateManager.Instance().[_factoryType.ToString()][_idx] };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.UPGRADE_INFO, data, raiseEventOptions, SendOptions.SendReliable);





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




    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.UPGRADE_INFO)
        {
            // �ٸ� �÷��̾���� ȣ���� RPC�� ���� ���� ����
            object[] receivedData = (object[])photonEvent.CustomData;
            float receivedMeter = (float)receivedData[0];

            UIManager.Instance().SetText(UIManager.Instance().distance03, (int)(receivedMeter * 100) + "m");
        }
    }



    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

}