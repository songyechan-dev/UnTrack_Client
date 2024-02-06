using ExitGames.Client.Photon;

using LeeYuJoung;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSON;

using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static FactoryManager;


public class MapCreator : MonoBehaviour
{
    [System.Serializable]
    private class ToBeCreatedPos
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;
        public Vector3 v4;
    }

    [SerializeField]
    private List<ToBeCreatedPos> toBeCreatedPos = new List<ToBeCreatedPos>();

    private List<List<int>> mapInfo = new List<List<int>>();
    [SerializeField]
    private int mapY;
    [SerializeField]
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
    public GameObject minePrefab;
    [Header("")]
    public GameObject dynamiteMachinePrefab;
    public GameObject productionMachinePrefab;
    public GameObject waterTankPrefab;
    public GameObject enginePrefab;
    public GameObject storagePrefab;
    public GameObject axPrefab;
    public GameObject pickPrefab;

    GameObject obStoneObject;
    GameObject trackObject;
    GameObject obTreeObject;

    GameObject factoryObject;
    GameObject engineObject;
    GameObject storageObject;

    GameObject axObject;
    GameObject pickObject;

    [Header("")]
    public TrackManager trackManager;
    public PhotonObjectCreator photonObjectCreator;
    private PhotonView pv;
    private GameObject planeObject;
    private static MapCreator instance;

    Dictionary<int,List<int>> planeList = new Dictionary<int,List<int>>();
    int treeCount = 0;
    int stoneCount = 0;

    GameObject mapEnvironmentPrefab;

    private void Awake()
    {
        //방장인지 확인
        if (PhotonNetwork.IsMasterClient)
        {
            planeList.Clear();
            treeCount = 0;
            stoneCount = 0;

            //MapLoad();
        }
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
        photonObjectCreator = GameObject.Find("PhotonObjectCreator").GetComponent<PhotonObjectCreator>();
        pv = GetComponent<PhotonView>();
        photonObjectCreator.Create("Player", new Vector3(0, 0.5f, 0));
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance().GameStart();
            QuestManager.Instance().Init();
        }
    }

    private void MapEnvironmentCreate_Master()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(mapEnvironmentPrefab.name));
    }

    private void MapEnvironmentCreate_Others()
    {

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
        int createdFactoryCount = 0;
        int factoriesCreatedX = 0;
        bool notCreateOB = false;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapInfo = "0";
                
                // TODO : 랜덤 범위 지정 필요(2024.01.14) - 송예찬 MapTool.cs
                if (!isCreatedObStone && !isCreatedObTree)
                {
                    //장애물 생성x
                    if (y > mapHeight - 15)
                    {
                        notCreateOB = true;
                    }
                    if (!notCreateOB)
                    {
                        int randomCount = Random.Range(0, 70);
                        randomXcount = Random.Range(3, 7);
                        randomYcount = Random.Range(1, 4);
                        if (randomCount < 1)
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
                    
                }
                notCreateOB = false;

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
                        if (mapInfo == ((int)MapInfo.Type.OBSTACLE_TREE).ToString())
                        {
                            int rand = Random.Range(0, 2);
                            if (rand > 0)
                            {
                                mapInfo = ((int)MapInfo.Type.PLANE).ToString();
                            }
                        }

                    }
                }
                if (!isCreatedFactory || !isCreatedEngine || !isCreatedStorage)
                {
                    if (y == mapHeight -3)
                    {
                        if (!isCreatedEngine && x>3)
                        {
                            factoriesCreatedX = x;
                            mapInfo = ((int)(MapInfo.Type.ENGINE)).ToString();
                            isCreatedEngine = true;
                            Debug.Log("엔진 생성");
                        }
						else if (!isCreatedStorage && x >= factoriesCreatedX + 6)
                        {
                            factoriesCreatedX = x;
                            mapInfo = ((int)(MapInfo.Type.STORAGE)).ToString();
                            isCreatedStorage = true;
                            Debug.Log("스토리지 생성");
                        }
                        else if (round == 1 && !isCreatedFactory  &&x >= factoriesCreatedX+6)
                        {
                            factoriesCreatedX = x;
                            createdFactoryCount++;
                            mapInfo = ((int)(MapInfo.Type.FACTORY)).ToString();
                            if (createdFactoryCount == 2)
                            {
                                isCreatedFactory = true;
                                factoriesCreatedX = 0;
                            }
                        }
                        else if (round != 1 && factoryCreatedCount < StateManager.Instance().waterTanks.Count + StateManager.Instance().productionMachines.Count + StateManager.Instance().dynamiteMachines.Count && x>factoriesCreatedX +6)
                        {
                            factoriesCreatedX = x;
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

                //출발TrackObject
                if ((x >= MapInfo.defaultStartTrackX -2 && x <= MapInfo.defaultStartTrackX +5) && (y > MapInfo.defaultStartTrackZ && y <= MapInfo.defaultStartTrackZ +5))
                {
                    mapInfo = ((int)(MapInfo.Type.START_MINE)).ToString();
                }
                if ((x >= MapInfo.finishStartTrackX - 2 && x <= MapInfo.finishStartTrackX + 5) && (y > MapInfo.finishEndTrackZ && y <= MapInfo.finishEndTrackZ + 5))
                {
                    mapInfo = ((int)(MapInfo.Type.END_MINE)).ToString();
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

    //TODO : 2024.02.01 송예찬 수정 필요
    public void CreateOB()
    {
        // stone 갯수, wood 갯수 파악 필요
        if (planeList.Count <= 0)
        {
            for (int y = 0; y < mapY; y++)
            {
                List<int> planeXList = new List<int>();
                for (int x = 0; x < mapX; x++)
                {
                    if (mapInfo[y][x] == (int)MapInfo.Type.PLANE)
                    {
                        planeXList.Add(x);
                    }

                    if (mapInfo[y][x] == (int)MapInfo.Type.OBSTACLE_STONE)
                    {
                        stoneCount++;
                    }
                    if (mapInfo[y][x] == (int)MapInfo.Type.OBSTACLE_TREE)
                    {
                        treeCount++;
                    }
                }
                if (planeXList.Count > 1)
                {
                    planeList.Add(y, planeXList);
                }
            }
        }
        if (toBeCreatedPos.Count <= 0)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                if (planeList.ContainsKey(i))
                {
                    for (int j = 0; j < planeList[i].Count; j++)
                    {
                        if (planeList[i].Count > 1 && i + 1 < mapHeight && planeList.ContainsKey(i + 1) && planeList[i + 1].Count > 1)
                        {
                            planeList[i].Sort();
                            planeList[i + 1].Sort();
                            if (j + 1 < planeList[i].Count && j + 1 < planeList[i + 1].Count && (planeList[i][j] + 1) == planeList[i][j + 1] && (planeList[i + 1][j] + 1) == planeList[i + 1][j + 1])
                            {
                                Vector3 first = new Vector3(j, 0.5f, i);
                                Vector3 secound = new Vector3(j + 1, 0.5f, i);
                                Vector3 third = new Vector3(j, 0.5f, i + 1);
                                Vector3 forth = new Vector3(j + 1, 0.5f, i + 1);
                                ToBeCreatedPos _tobeCreatedPos = new ToBeCreatedPos();
                                _tobeCreatedPos.v1 = first;
                                _tobeCreatedPos.v2 = secound;
                                _tobeCreatedPos.v3 = third;
                                _tobeCreatedPos.v4 = forth;
                                toBeCreatedPos.Add(_tobeCreatedPos);
                            }
                        }
                    }
                }
            }
        }
        if (treeCount > stoneCount)
        {
            int rand = Random.Range(0, toBeCreatedPos.Count);
            RaycastHit hit;
            Ray rayV1 = new Ray(new Vector3(toBeCreatedPos[rand].v1.x, 0, toBeCreatedPos[rand].v1.z), Vector3.up);
            Ray rayV2 = new Ray(new Vector3(toBeCreatedPos[rand].v2.x, 0, toBeCreatedPos[rand].v2.z), Vector3.up);
            Ray rayV3 = new Ray(new Vector3(toBeCreatedPos[rand].v3.x, 0, toBeCreatedPos[rand].v3.z), Vector3.up);
            Ray rayV4 = new Ray(new Vector3(toBeCreatedPos[rand].v4.x, 0, toBeCreatedPos[rand].v4.z), Vector3.up);

            //생성전 오브젝트들이 생성이 되어있는지 확인
            if (Physics.Raycast(rayV1, out hit,1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV2, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV3, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV4, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            CreateObject_Master(toBeCreatedPos[rand].v1.x, toBeCreatedPos[rand].v1.y, toBeCreatedPos[rand].v1.z, obStonePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v2.x, toBeCreatedPos[rand].v2.y, toBeCreatedPos[rand].v2.z, obStonePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v3.x, toBeCreatedPos[rand].v3.y, toBeCreatedPos[rand].v3.z, obStonePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v4.x, toBeCreatedPos[rand].v4.y, toBeCreatedPos[rand].v4.z, obStonePrefab.name);
            stoneCount+=4;
            toBeCreatedPos.RemoveAt(rand);
        }
        else
        {
            int rand = Random.Range(0, toBeCreatedPos.Count);
            RaycastHit hit;
            Ray rayV1 = new Ray(new Vector3(toBeCreatedPos[rand].v1.x, 0, toBeCreatedPos[rand].v1.z), Vector3.up);
            Ray rayV2 = new Ray(new Vector3(toBeCreatedPos[rand].v2.x, 0, toBeCreatedPos[rand].v2.z), Vector3.up);
            Ray rayV3 = new Ray(new Vector3(toBeCreatedPos[rand].v3.x, 0, toBeCreatedPos[rand].v3.z), Vector3.up);
            Ray rayV4 = new Ray(new Vector3(toBeCreatedPos[rand].v4.x, 0, toBeCreatedPos[rand].v4.z), Vector3.up);

            //생성전 오브젝트들이 생성이 되어있는지 확인
            if (Physics.Raycast(rayV1, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV2, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV3, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            else if (Physics.Raycast(rayV4, out hit, 1f))
            {
                Debug.Log("통과");
                CreateOB();
                return;
            }
            CreateObject_Master(toBeCreatedPos[rand].v1.x, toBeCreatedPos[rand].v1.y, toBeCreatedPos[rand].v1.z, obTreePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v2.x, toBeCreatedPos[rand].v2.y, toBeCreatedPos[rand].v2.z, obTreePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v3.x, toBeCreatedPos[rand].v3.y, toBeCreatedPos[rand].v3.z, obTreePrefab.name);
            CreateObject_Master(toBeCreatedPos[rand].v4.x, toBeCreatedPos[rand].v4.y, toBeCreatedPos[rand].v4.z, obTreePrefab.name);
            treeCount+=4;
            toBeCreatedPos.RemoveAt(rand);
        }
        
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
        int createdFactoryCount = 0;
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                pv.RPC("CreatePlane_Master",RpcTarget.MasterClient,x,y,z);
                if (mapInfo[i][j] == (int)MapInfo.Type.OBSTACLE_STONE)
                {
                    if ((j >= MapInfo.startPosition.x && j < MapInfo.defaultEndTrackX) && (i < MapInfo.defaultStartTrackZ || i == MapInfo.defaultStartTrackZ + 1))
                    {
                        //pv.RPC("CreatePlane_Master", RpcTarget.MasterClient, x, y, z)
                    }
                    else
                    {
                        prevCreatedYPos = planeObject.transform.position.y;
                        int yCount = Random.Range(1, 5);
                        for (int k = 1; k <= yCount; k++)
                        {
                            GameObject _obj = PhotonNetwork.Instantiate(obStonePrefab.name, new Vector3(x * objScale * 10, (k == 1 ? planeObject.transform.position.y + 0.5f : prevCreatedYPos + 1), z * objScale * 10), Quaternion.identity);
                            _obj.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                            prevCreatedYPos = _obj.transform.position.y;
                        }
                    }
                    
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.OBSTACLE_TREE)
                {
                    if ((j >= MapInfo.startPosition.x && j < MapInfo.defaultEndTrackX) && (i < MapInfo.defaultStartTrackZ || i == MapInfo.defaultStartTrackZ + 1))
                    {
                        
                    }
                    else
                    {
                        CreateObject_Master(x, y, z, obTreePrefab.name);
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.START_MINE)
                {
                    if (j == MapInfo.defaultStartTrackX + 1 && i == MapInfo.defaultStartTrackZ + 3)
                    {
                        CreateObject_Master(x, y, z, minePrefab.name);
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.END_MINE)
                {
                    if (j == MapInfo.finishStartTrackX + 1 && i == MapInfo.finishStartTrackZ + 3)
                    {
                        CreateObject_Master(x, y, z, minePrefab.name);
                    }
                }
                

                else if (mapInfo[i][j] == (int)MapInfo.Type.TRACK || mapInfo[i][j] == (int)MapInfo.Type.FiNISH_TRACK)
                {
                    CreateTrack_Master(x, y, z, trackPrefab.name, i, j);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.FACTORY)
                {
                    if (round == 1)
                    {
                        if (createdFactoryCount == 0)
                        {
                            CreateFactory_Master(x, y, z, productionMachinePrefab.name, FACTORYTYPE.ProductionMachine);
                        }
                        else
                        {
                            CreateFactory_Master(x, y, z, waterTankPrefab.name, FACTORYTYPE.WaterTank);
                        }
                        createdFactoryCount++;
                    }
                    else
                    {
                        if (StateManager.Instance().productionMachines.Count > 0)
                        {
                            CreateFactory_Master(x, y, z, productionMachinePrefab.name, FACTORYTYPE.ProductionMachine);
                        }
                        else if (StateManager.Instance().waterTanks.Count > 0)
                        {
                            CreateFactory_Master(x, y, z, waterTankPrefab.name, FACTORYTYPE.WaterTank);
                        }
                        else if (StateManager.Instance().dynamiteMachines.Count > 0)
                        {
                            CreateFactory_Master(x, y, z, dynamiteMachinePrefab.name, FACTORYTYPE.DynamiteMachine);
                        }
                    }
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.STORAGE)
                {
                    CreateObject_Master(x, y, z, storagePrefab.name);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.ENGINE)
                {
                    Debug.Log("엔진생성 호출");
                    CreateObject_Master(x, y, z, enginePrefab.name);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.Ax)
                {
                    Debug.Log("Ax 생성 호출");
                    CreateObject_Master(x, y, z, axPrefab.name);
                }
                else if (mapInfo[i][j] == (int)MapInfo.Type.Pick)
                {
                    CreateObject_Master(x, y, z, pickPrefab.name);
                }
                x++;
            }
            x = originX;
            z++;

        }
        //EditorUtility.SetDirty(mapParent.gameObject);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        MapInfo.endPositionX = mapX;
        MapInfo.endPositionZ = mapY;
        SetEndPosition(mapX, mapY);
        Debug.Log(MapInfo.endPositionX +", "+ MapInfo.endPositionZ);
        StateManager.Instance().sceneFactorys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Factory"));
        pv.RPC("SetSceneFactorys_Others", RpcTarget.Others);
        Debug.Log("찾았다 ::::" + StateManager.Instance().sceneFactorys.Count);
    }
    [PunRPC]
    void SetSceneFactorys_Others()
    {
        StateManager.Instance().sceneFactorys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Factory"));
        Debug.Log("찾았다 ::::" + StateManager.Instance().sceneFactorys.Count);
    }

    [PunRPC]
    void CreatePlane_Master(float x, float y, float z)
    {
        planeObject = Instantiate(planePrefab);
        planeObject.tag = "Plane";
        planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
        planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
        pv.RPC("CreatePlane_Others", RpcTarget.Others, x, y, z);
    }
    [PunRPC]
    void CreatePlane_Others(float x, float y, float z)
    {
        planeObject = Instantiate(planePrefab);
        planeObject.tag = "Plane";
        planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
        planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
    }
    void CreateObject_Master(float x, float y, float z,string name)
    {
        Debug.Log("마스터 호출");
        GameObject _obj = PhotonNetwork.Instantiate(name, new Vector3(x,MapInfo.startPosition.y + 0.5f, z), Quaternion.identity);
        
        _obj.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
    }

    void CreateFactory_Master(float x, float y,float z, string name,FactoryManager.FACTORYTYPE _factoryType)
    {
        factoryObject = PhotonNetwork.Instantiate(name, new Vector3(x * objScale * 10, dynamiteMachinePrefab.transform.localScale.y / 2, z * objScale * 10), Quaternion.identity);
        factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
        factoryObject.AddComponent<FactoryManager>();
        factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
        factoryObject.GetComponent<FactoryManager>().factoryType = _factoryType;
        factoryObject.GetComponent<FactoryManager>().Init();
        pv.RPC("CreateFactory_Others", RpcTarget.Others, x, y, z, factoryObject.GetComponent<PhotonView>().ViewID, _factoryType);
    }

    [PunRPC]
    void CreateFactory_Others(float x, float y, float z, int viewID, FactoryManager.FACTORYTYPE _factoryType)
    {
        factoryObject = PhotonView.Find(viewID).gameObject;
        factoryObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
        factoryObject.AddComponent<FactoryManager>();
        factoryObject.GetComponent<FactoryManager>().dataPath = "FactoryData";
        factoryObject.GetComponent<FactoryManager>().factoryType = _factoryType;
        factoryObject.GetComponent<FactoryManager>().Init();
    }


    void CreateTrack_Master(float x, float y, float z, string name,int i, int j)
    {
        bool isTrack = false;

        trackObject = PhotonNetwork.Instantiate(name, new Vector3(x * objScale * 10, planeObject.transform.localPosition.y +(0.05f), z * objScale * 10),Quaternion.Euler(new Vector3(0,MapInfo.startTrackYRotation,0)));
        trackObject.AddComponent<TrackInfo>();
        trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(trackPrefab.transform.localRotation.x, trackPrefab.transform.rotation.y, trackPrefab.transform.localRotation.z));
        trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
        trackObject.tag = "Track";
        MapInfo.trackYscale = trackPrefab.transform.localScale.y;
        //trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
        if (mapInfo[i][j] == (int)MapInfo.Type.TRACK)
        {
            trackObject.transform.localEulerAngles = new Vector3(MapInfo.trackDefaultXRotation, MapInfo.startTrackYRotation, MapInfo.trackDefaultZRotation);
            trackManager.finalTrack = trackObject;
            isTrack = true;
        }
        else
        {
            trackObject.transform.localEulerAngles = new Vector3(MapInfo.trackDefaultXRotation, MapInfo.endTrackYRotation, MapInfo.trackDefaultZRotation);
            trackObject.GetComponent<TrackInfo>().isFinishedTrack = true;
            isTrack = false;
        }
        pv.RPC("CreateTrack_Others", RpcTarget.Others,x, y, z, trackObject.GetComponent<PhotonView>().ViewID, i, j, isTrack);
    }

    [PunRPC]
    void CreateTrack_Others(float x, float y, float z, int ViewID, int i, int j,bool isTrack)
    {
        Debug.Log("map count 는 ==" + mapInfo.Count);
        trackObject = PhotonView.Find(ViewID).gameObject;
        trackObject.AddComponent<TrackInfo>();
        trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, trackPrefab.transform.rotation.y, MapInfo.trackDefaultZRotation));
        trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
        trackObject.tag = "Track";
        MapInfo.trackYscale = trackPrefab.transform.localScale.y;
        trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
        if (isTrack)
        {
            trackObject.transform.localEulerAngles = new Vector3(MapInfo.trackDefaultXRotation, MapInfo.startTrackYRotation, MapInfo.trackDefaultZRotation);
            trackManager.finalTrack = trackObject;
        }
        else
        {
            trackObject.transform.localEulerAngles = new Vector3(MapInfo.trackDefaultXRotation, MapInfo.endTrackYRotation, MapInfo.trackDefaultZRotation);
            trackObject.GetComponent<TrackInfo>().isFinishedTrack = true;
        }
    }

    void AddComponent(Transform _tr, FactoryManager.FACTORYTYPE _factoryType)
    {
        int viewId = _tr.GetComponent<PhotonView>().ViewID;
        object[] data = new object[] { viewId, (int)_factoryType };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.FACTORY_INFO, data, raiseEventOptions, SendOptions.SendReliable);
    }

    void SetEndPosition(int x, int z)
    {
        object[] data = new object[] { x, z };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.MAP_END_POS, data, raiseEventOptions, SendOptions.SendReliable);
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.FACTORY_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int viewId = (int)receivedData[0];
            FACTORYTYPE _factoryType = (FACTORYTYPE)(int)receivedData[1];
            PhotonView factoryView = PhotonView.Find(viewId);

            factoryView.AddComponent<FactoryManager>();
            factoryView.GetComponent<FactoryManager>().dataPath = "FactoryData";
            factoryView.GetComponent<FactoryManager>().factoryType = _factoryType;
            factoryView.GetComponent<FactoryManager>().Init();
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.MAP_END_POS)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int x = (int)receivedData[0];
            int z = (int)receivedData[1];
            MapInfo.endPositionX = x;
            MapInfo.endPositionZ = z;
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
