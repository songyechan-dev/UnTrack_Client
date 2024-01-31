using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject factoriesObject = PhotonNetwork.Instantiate("FactoriesObject", new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, 0.75f, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10),Quaternion.Euler(new Vector3(0, MapInfo.startTrackYRotation, 0)));
        factoriesObject.AddComponent<FactoriesObjectManager>();
        factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesObject.GetComponent<FactoriesObjectManager>().Init();
        count++;
    }

    public void FirstCreate_Master()
    {
        GameObject factoriesObject = Instantiate(factoriesObjectPrefab, new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, 0.75f, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10), Quaternion.Euler(new Vector3(0, MapInfo.startTrackYRotation, 0)));
        factoriesObject.AddComponent<FactoriesObjectManager>();
        factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
        factoriesObject.GetComponent<FactoriesObjectManager>().Init();
        GameManager.Instance().firstFactoriesObject = factoriesObject;
        factoriesObject.GetComponent<MeshRenderer>().enabled = false;
        factoriesObject.layer = LayerMask.NameToLayer("FactoriesObject_First");
        factoriesObject.tag = "FactoriesObject_First";
        factoriesObject.transform.Find("Sensor").gameObject.layer = LayerMask.NameToLayer("FactoriesObject_First");
        count++;
    }

    

public void Init()
    {
        count = 0;
        createdTime = 15f;
    }

}
