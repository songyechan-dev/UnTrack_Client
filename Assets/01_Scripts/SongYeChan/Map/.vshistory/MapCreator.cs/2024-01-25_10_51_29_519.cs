using JetBrains.Annotations;
using LeeYuJoung;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class MapCreator : MonoBehaviour
{
    private List<List<int>> mapInfo = new List<List<int>>();
    private int mapY;
    private int mapX;
    private bool isMapDataLoaded = false;
    private float objScale = MapInfo.objScale;

    private string trackYRotationInfoFileName = MapInfo.trackYRotationInfoFileName;

    private Vector3 startPosition = MapInfo.startPosition;
    private Vector3 startTrackRotation = MapInfo.startTrackRotation;
    private Vector3 endTrackRotation = MapInfo.endTrackRotation;

    private string startTrackYRotationKeyName = MapInfo.startTrackYRotationKeyName;
    private string endTrackYRotationKeyName = MapInfo.endTrackYRotationKeyName;

    private int mapWidth = MapInfo.mapWidth;
    private int mapHeight = MapInfo.mapHeight;

    private bool isCreatedFactory;
    private bool isCreatedStorage;
    private bool isCreatedEngine;

    private Dictionary<string,float> rotationInfoDict = new Dictionary<string,float>();

    private int round =1;

    private string content = "";
    private string rotationInfo = $"{MapInfo.startTrackYRotationKeyName},{MapInfo.startTrackYRotation}\n{MapInfo.endTrackYRotationKeyName},{MapInfo.endTrackYRotation}";

    [Header("Prefabs")]
    public GameObject planePrefab;
    public GameObject obStonePrefab;
    public GameObject obTreePrefab;
    public GameObject trackPrefab;
    [Header("")]
    public GameObject dynamiteMachinePrefab;
    public GameObject productionMachinePrefab;
    public GameObject waterTankPrefab;
    public GameObject enginePrefab;
    public GameObject storagePrefab;
    public GameObject axPrefab;
    public GameObject pickPrefab;
    [Header("")]
    public TrackManager trackManager;
    public PhotonObjectCreator photonObjectCreator;
    private PhotonView pv;
    private GameObject planeObject;

    private static MapCreator instance;
    public static MapCreator Instance()
    {
        return instance;
    }

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
        //방장인지 확인
        if (PhotonNetwork.IsMasterClient)
        {
            MapLoad();
        }
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
        photonObjectCreator = GameObject.Find("PhotonObjectCreator").GetComponent<PhotonObjectCreator>();
        pv = GetComponent<PhotonView>();
        photonObjectCreator.Create("Player", new Vector3(0, 0.5f, 0));
    }

    private void MapDataCreate()
    {
        string mapInfo = "0";

        int randomXcount = 0;
        int originX = 0;
        int originY = 0;
        int randomYcount = 0;
        int factoryCreatedCount = 0;
        bool isCreatedObStone = false;
        bool isCreatedObTree = false;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapInfo = "0";
                
                // TODO : 랜덤 범위 지정 필요(2024.01.14) - 송예찬 MapTool.cs
                if (!isCreatedObStone && !isCreatedObTree && y +3 < mapHeight)
                {
                    int randomCount = Random.Range(0, 70);
                    randomXcount = Random.Range(3, 7);
                    randomYcount = Random.Range(1, 4);
                    if (randomCount <1)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            isCreatedObStone = true;
                        }
                        else
                        {
                            isCreatedObTree = true;
                        }
                        originX = x;
                        originY = y;
                    }
                }

                if (isCreatedObStone || isCreatedObTree)
                {
                    if (y > (randomYcount + originY))
                    {
                        randomXcount = 0;
                        randomYcount = 0;
                        originX = 0;
                        originY = 0;
                        isCreatedObStone = false;
                        isCreatedObTree = false;
                    }
                    else if (x <= (randomXcount + originX) && x >= originX)
                    {
                        mapInfo = isCreatedObStone ? ((int)(MapInfo.Type.OBSTACLE_STONE)).ToString() : ((int)(MapInfo.Type.OBSTACLE_TREE)).ToString();
                    }
                }
                if (!isCreatedFactory || !isCreatedEngine || !isCreatedStorage)
                {
                    int rand = Random.Range(0, 500);
                    if (rand < 100)
                    {
                        if (!isCreatedEngine)
                        {
                            mapInfo = ((int)(MapInfo.Type.ENGINE)).ToString();
                            isCreatedEngine = true;
                            Debug.Log("엔진 생성");
                        }
                        else if (!isCreatedStorage)
                        {
                            mapInfo = ((int)(MapInfo.Type.STORAGE)).ToString();
                            isCreatedStorage = true;
                            Debug.Log("스토리지 생성");
                        }
                        else if (round == 1 && !isCreatedFactory)
                        {
                            mapInfo = ((int)(MapInfo.Type.FACTORY)).ToString();
                            isCreatedFactory = true;
                            Debug.Log("팩토리 생성");
                        }
                        else if (round != 1 && factoryCreatedCount < StateManager.Instance().waterTanks.Count + StateManager.Instance().productionMachines.Count + StateManager.Instance().dynamiteMachines.Count)
                        {
                            mapInfo = ((int)(MapInfo.Type.FACTORY)).ToString();
                            factoryCreatedCount++;
                            Debug.Log("팩토리 생성");
                        }
                    }
                }
                //출발Track
                if ((x >= MapInfo.defaultStartTrackX && x <= MapInfo.defaultEndTrackX) && (y >= MapInfo.defaultStartTrackZ && y <= MapInfo.defaultEndTrackZ))
                {
                    mapInfo = ((int)(MapInfo.Type.TRACK)).ToString();
                }

                //도착Track
                if ((x >= MapInfo.finishStartTrackX && x <= MapInfo.finishEndTrackX) && (y >= MapInfo.finishStartTrackZ && y <= MapInfo.finishEndTrackZ))
                {
                    mapInfo = ((int)(MapInfo.Type.FiNISH_TRACK)).ToString();
                }
                if (x == MapInfo.defaultStartTrackX - 1 && y == MapInfo.defaultStartTrackZ - 1)
                {
                    mapInfo = ((int)(MapInfo.Type.Ax)).ToString();
                }
                if (x == MapInfo.defaultStartTrackX - 2 && y == MapInfo.defaultStartTrackZ - 2)
                {
                    mapInfo = ((int)(MapInfo.Type.Pick)).ToString();
                }
                if (x == 0)
                {
                    content += mapInfo;
                }
                else
                {
                    content += $",{mapInfo}";
                }

            }
            if (y < mapHeight - 1)
            {
                content += "\n";
            }
        }
        Debug.Log(content);
        isCreatedFactory = false;
        isCreatedEngine = false;
        isCreatedStorage = false;
    }

    private IEnumerator DataLoad()
    {
        MapDataCreate();
        if (!string.IsNullOrEmpty(content))
        {
            string[] lines = content.Split('\n');
            Debug.Log("length :::::::" +lines.Length);
            foreach (string line in lines)
            {
                List<int> row = new List<int>();
                string[] values = line.Split(',');
                foreach (string value in values)
                {
                    row.Add(int.Parse(value));
                }
                mapInfo.Add(row);
            }
            mapY = mapInfo.Count;
            mapX = mapInfo[0].Count;
        }
        else
        {
            Debug.Log("로드 에러");
            yield return null; // 실패 시 종료
        }
        if (!string.IsNullOrEmpty(rotationInfo))
        {
            Debug.Log("NotNull");
            rotationInfoDict.Clear();
            string[] rotationLines = rotationInfo.Split('\n');
            for (int i = 0; i < rotationLines.Length; i++)
            {
                string[] values = rotationLines[i].Split(',');
                if (values.Length >= 2)
                {
                    string key = values[0];
                    float value = float.Parse(values[1]);
                    rotationInfoDict.Add(key, value);
                    isMapDataLoaded = true;
                }
                else
                {
                    Debug.LogError("CSV Format 이상함: " + rotationLines[i]);
                }
            }
        }
        else
        {
            Debug.Log("로드 에러");
            yield return null; // 실패 시 종료
        }

    }

    private IEnumerator WaitUntilMapDataLoaded()
    {
        isMapDataLoaded = false;
        yield return StartCoroutine(DataLoad());

        // Data 로딩이 완료될 때까지 대기
        while (!isMapDataLoaded)
        {
            yield return null;
        }
        Create();
    }

    public void MapLoad()
    {
        round = GameManager.Instance().GetRound();
        Debug.Log("Round :::" + round);
        StartCoroutine(WaitUntilMapDataLoaded());
    }

    public void Create()
    {
        if (mapInfo.Count <= 0)
        {
            return;
        }
        GameObject mapParent = Instantiate(new GameObject());
        FactoriesObjectCreator.Instance().mapParent = mapParent.transform;
        mapParent.name = "MapParent";
        float originX = startPosition.x;

        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;

        float prevCreatedYPos;

        GameObject obStoneObject;
        GameObject trackObject;
        GameObject obTreeObject;

        GameObject factoryObject;
        GameObject engineObject;
        GameObject storageObject;

        GameObject axObject;
        GameObject pickObject;
        

        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                pv.RPC("CreatePlane_Master",RpcTarget.MasterClient,x,y,z);
                CreatePlane_Master(x, y, z);
                if (mapInfo[i][j] == (int)MapInfo.Type.OBSTACLE_STONE)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++)
                    {
                        obStoneObject = PhotonNetwork.Instantiate("Stone", new Vector3(x * objScale * 10, k == 0 ? planeObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10),Quaternion.identity);
                        obStoneObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                        prevCreatedYPos = obStoneObject.transform.position.y;
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.OBSTACLE_TREE)
                {
                    obTreeObject = Instantiate(obTreePrefab, mapParent.transform);
                    obTreeObject.transform.position = new Vector3(x * objScale * 10, obTreePrefab.transform.localScale.y / 2, z * objScale * 10);
                    obTreeObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.TRACK || mapInfo[i][j] == (int)MapInfo.Type.FiNISH_TRACK)
                {
                    trackObject = Instantiate(trackPrefab, mapParent.transform);
                    trackObject.AddComponent<TrackInfo>();
                    trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                    trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    trackObject.tag = "Track";
                    MapInfo.trackYscale = trackPrefab.transform.localScale.y;
                    trackObject.transform.position = new Vector3(x * objScale * 10, trackPrefab.transform.localScale.y / 2, z * objScale * 10);
                    trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
                    if (mapInfo[i][j] == (int)MapInfo.Type.TRACK)
                    {
                        trackObject.transform.localEulerAngles = new Vector3(0, rotationInfoDict[startTrackYRotationKeyName], 0);
                        trackManager.finalTrack = trackObject;
                    }
                    else
                    {
                        trackObject.transform.localEulerAngles = new Vector3(0, rotationInfoDict[endTrackYRotationKeyName], 0);
                        trackObject.GetComponent<TrackInfo>().isFinishedTrack = true;
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.FACTORY)
                {
                    if (round == 1)
                    {
                        factoryObject = Instantiate(productionMachinePrefab, mapParent.transform);
                        factoryObject.transform.position = new Vector3(x * objScale * 10, productionMachinePrefab.transform.localScale.y / 2, z * objScale * 10);
                        factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                        factoryObject.AddComponent<FactoryManager>();
                        factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
                        factoryObject.GetComponent<FactoryManager>().factoryType = FactoryManager.FACTORYTYPE.ProductionMachine;
                        factoryObject.GetComponent<FactoryManager>().Init();
                        factoryObject = null;
                    }
                    else
                    {
                        if (StateManager.Instance().productionMachines.Count > 0)
                        {
                            factoryObject = Instantiate(productionMachinePrefab, mapParent.transform);
                            factoryObject.transform.position = new Vector3(x * objScale * 10, productionMachinePrefab.transform.localScale.y / 2, z * objScale * 10);
                            factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                            factoryObject.AddComponent<FactoryManager>();
                            factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
                            factoryObject.GetComponent<FactoryManager>().factoryType = FactoryManager.FACTORYTYPE.ProductionMachine;
                            factoryObject.GetComponent<FactoryManager>().Init();
                            factoryObject = null;
                        }
                        else if (StateManager.Instance().waterTanks.Count > 0)
                        {
                            factoryObject = Instantiate(waterTankPrefab, mapParent.transform);
                            factoryObject.transform.position = new Vector3(x * objScale * 10, waterTankPrefab.transform.localScale.y / 2, z * objScale * 10);
                            factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                            factoryObject.AddComponent<FactoryManager>();
                            factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
                            factoryObject.GetComponent<FactoryManager>().factoryType = FactoryManager.FACTORYTYPE.WaterTank;
                            factoryObject.GetComponent<FactoryManager>().Init();
                            factoryObject = null;
                        }
                        else if (StateManager.Instance().dynamiteMachines.Count > 0)
                        {
                            factoryObject = Instantiate(dynamiteMachinePrefab, mapParent.transform);
                            factoryObject.transform.position = new Vector3(x * objScale * 10, dynamiteMachinePrefab.transform.localScale.y / 2, z * objScale * 10);
                            factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                            factoryObject.AddComponent<FactoryManager>();
                            factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
                            factoryObject.GetComponent<FactoryManager>().factoryType = FactoryManager.FACTORYTYPE.DynamiteMachine;
                            factoryObject.GetComponent<FactoryManager>().Init();
                            factoryObject = null;
                        }
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.STORAGE)
                {
                    storageObject = Instantiate(storagePrefab, mapParent.transform);
                    storageObject.transform.position = new Vector3(x * objScale * 10, storagePrefab.transform.localScale.y / 2, z * objScale * 10);
                    storageObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);

                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.ENGINE)
                {
                    Debug.Log("엔진생성 호출");
                    engineObject = Instantiate(enginePrefab, mapParent.transform);
                    engineObject.transform.position = new Vector3(x * objScale * 10, enginePrefab.transform.localScale.y / 2, z * objScale * 10);
                    engineObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.Ax)
                {
                    Debug.Log("Ax 생성 호출");
                    axObject = Instantiate(axPrefab, mapParent.transform);
                    axObject.transform.position = new Vector3(x * objScale * 10, axPrefab.transform.localScale.y / 2, z * objScale * 10);
                    axObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.Pick)
                {
                    pickObject = Instantiate(pickPrefab, mapParent.transform);
                    pickObject.transform.position = new Vector3(x * objScale * 10, pickPrefab.transform.localScale.y / 2, z * objScale * 10);
                    pickObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                }
                x++;
            }
            x = originX;
            z++;

        }
        //EditorUtility.SetDirty(mapParent.gameObject);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        
    }

    [PunRPC]
    void CreatePlane_Master(float x, float y, float z)
    {
        planeObject = Instantiate(planePrefab);
        planeObject.tag = "Plane";
        planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
        planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
        pv.RPC("CreatePlane_Others", RpcTarget.OthersBuffered, x, y, z);
    }

    [PunRPC]
    void CreatePlane_Others(float x, float y, float z)
    {

        planeObject = Instantiate(planePrefab);
        planeObject.tag = "Plane";
        planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
        planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
    }
}
