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
        InvokeRepeating("Create", 0f, createdTime);
    }
    
    public void Create()
    {
        if (GameManager.Instance().gameState.Equals(GameManager.GameState.GameStart))
        {
            
            GameObject factoriesObject = Instantiate(factoriesObjectPrefab, mapParent);
            factoriesObject.transform.position = new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, (factoriesObjectPrefab.transform.localScale.y / 2) + (MapInfo.trackYscale / 2) * 2, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10);
            factoriesObject.transform.localEulerAngles = new Vector3(0, MapInfo.startTrackYRotation, 0);
            factoriesObject.AddComponent<FactoriesObjectManager>();
            factoriesObject.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
            factoriesObject.GetComponent<FactoriesObjectManager>().Init();
            if (count <= 0)
            {
                GameManager.Instance().firstFactoriesObject = factoriesObject;
                factoriesObject.GetComponent<MeshRenderer>().enabled = false;
                Destroy(factoriesObject.GetComponent<Rigidbody>());
                GameObject factoriesObject2 = Instantiate(factoriesObjectPrefab, mapParent);
                factoriesObject2.transform.position = new Vector3(MapInfo.defaultStartTrackX * MapInfo.objScale * 10, (factoriesObjectPrefab.transform.localScale.y / 2) + (MapInfo.trackYscale / 2) * 2, MapInfo.defaultStartTrackZ * MapInfo.objScale * 10);
                factoriesObject2.transform.localEulerAngles = new Vector3(0, MapInfo.startTrackYRotation, 0);
                factoriesObject2.AddComponent<FactoriesObjectManager>();
                factoriesObject2.GetComponent<FactoriesObjectManager>().tagToBeDetected = "Track";
                factoriesObject2.GetComponent<FactoriesObjectManager>().Init();
            }
            count++;
        }
    }

    public void Init()
    {
        count = 0;
        createdTime = 15f;
    }
}
