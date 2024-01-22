using GoogleSheetsToUnity.ThirdPary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


// TODO : _round는 GameManager 의 Round로 변경 송예찬 2024.0.18
public class MapCreator : MonoBehaviour
{
    private List<List<int>> mapInfo = new List<List<int>>();
    private int mapY;
    private int mapX;
    private bool isMapCSVLoaded = false;
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
    public TrackManager trackManager;

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
    }

    private void MapDataCreate()
    {
        string mapInfo = "0";

        int randomXcount = 0;
        int originX = 0;
        int originY = 0;
        int randomYcount = 0;
        bool isCreatedObStone = false;
        bool isCreatedObTree = false;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapInfo = "0";
                if (round == 1)
                {
                    int rand = Random.Range(0, 1000);
                    if (!isCreatedFactory || !isCreatedEngine || isCreatedStorage)
                    {

                    }
                }
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
                        mapInfo = isCreatedObStone ? "1" : "2";
                    }
                }
                //출발Track
                if ((x >= MapInfo.defaultStartTrackX && x <= MapInfo.defaultEndTrackX) && (y >= MapInfo.defaultStartTrackZ && y <= MapInfo.defaultEndTrackZ))
                {
                    mapInfo = "3";
                }

                //도착Track
                if ((x >= MapInfo.finishStartTrackX && x <= MapInfo.finishEndTrackX) && (y >= MapInfo.finishStartTrackZ && y <= MapInfo.finishEndTrackZ))
                {
                    mapInfo = "4";
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

    }

    private IEnumerator CSVLoad()
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
                    isMapCSVLoaded = true;
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

    private IEnumerator WaitUntilMapCSVLoaded()
    {
        isMapCSVLoaded = false;
        yield return StartCoroutine(CSVLoad());

        // CSV 로딩이 완료될 때까지 대기
        while (!isMapCSVLoaded)
        {
            yield return null;
        }
        Create();
    }

    public void MapLoad()
    {
        round = GameManager.Instance().GetRound();
        Debug.Log("Round :::" + round);
        StartCoroutine(WaitUntilMapCSVLoaded());
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

        GameObject planeObject;
        GameObject obStoneObject;
        GameObject trackObject;
        GameObject obTreeObject;
        
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                planeObject = Instantiate(planePrefab, mapParent.transform);
                planeObject.tag = "Plane";
                planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[i][j] == (int)MapInfo.Type.OBSTACLE_STONE)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++)
                    {
                        obStoneObject = Instantiate(obStonePrefab, mapParent.transform);
                        obStoneObject.transform.position = new Vector3(x * objScale * 10, k == 0 ? planeObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10);
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
                x++;
            }
            x = originX;
            z++;
        }
        EditorUtility.SetDirty(mapParent.gameObject);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
