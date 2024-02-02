using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class FactoriesObjectCreator : MonoBehaviour
{
    private static FactoriesObjectCreator instance;
    public static FactoriesObjectCreator Instance()
    {
        return instance;
    }
    public GameObject factoriesObjectPrefab;
    public Transform mapParent;
    public float createdTime = 15f;
    public int count = 0;

    private MapCreator mapCreator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        if (GameManager.Instance().gameState.Equals(GameManager.GameState.GameStart) && PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("Create", 0f, createdTime);
        }
        
    }
    
    public void Create()
    {
        if (GameManager.Instance().gameState.Equals(GameManager.GameState.GameStart))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (count <= 0)
                {
                    FirstCreate_Master(); 
                }
                else
                {
                    Create_Master();
                }
                
            }
        }
    }

    public void Create_Master()
    {
        GameObject factoriesObject = PhotonNetwork.Instantiate("FactoriesObject", new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, 0.7f, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10),Quaternion.Euler(new Vector3(0, MapInfo.startTrackYRotation, 0)));
        factoriesObject.AddComponent<FactoriesObjectManager>();
        factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesObject.GetComponent<FactoriesObjectManager>().Init();
        object[] data = new object[] { factoriesObject.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORIES_OBJECT_INFO, data, raiseEventOptions, SendOptions.SendReliable);
        count++;
    }

    public void Create_Others(int viewID)
    {
        GameObject factoriesObject = PhotonView.Find(viewID).gameObject;
        factoriesObject.AddComponent<FactoriesObjectManager>();
        factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesObject.GetComponent<FactoriesObjectManager>().Init();
    }

    public void FirstCreate_Master()
    {
        GameObject factoriesFirstObject = Instantiate(factoriesObjectPrefab, new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, 0.7f, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10), Quaternion.Euler(new Vector3(0, MapInfo.startTrackYRotation, 0)));
        factoriesFirstObject.AddComponent<FactoriesObjectManager>();
        factoriesFirstObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesFirstObject.GetComponent<FactoriesObjectManager>().Init();
        GameManager.Instance().firstFactoriesObject = factoriesFirstObject;
        factoriesFirstObject.GetComponent<MeshRenderer>().enabled = false;
        factoriesFirstObject.layer = LayerMask.NameToLayer("FactoriesObject_First");
        factoriesFirstObject.transform.Find("Sensor").gameObject.layer = LayerMask.NameToLayer("FactoriesObject_First");
        factoriesFirstObject.tag = "FactoriesObject";

        GameObject factoriesObject = PhotonNetwork.Instantiate("FactoriesObject", new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, 0.7f, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10), Quaternion.Euler(new Vector3(0, MapInfo.startTrackYRotation, 0)));
        factoriesObject.AddComponent<FactoriesObjectManager>();
        factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesObject.GetComponent<FactoriesObjectManager>().Init();
        object[] data = new object[] { factoriesObject.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORIES_OBJECT_INFO, data, raiseEventOptions, SendOptions.SendReliable);

        count++;
    }

    

    public void Init()
    {
        count = 0;
        createdTime = 15f;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.FACTORIES_OBJECT_INFO)
        {
            // 다른 플레이어들이 호출한 RPC로 미터 값을 받음
            object[] receivedData = (object[])photonEvent.CustomData;
            int viewID = (int)receivedData[0];
            Create_Others(viewID);
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
