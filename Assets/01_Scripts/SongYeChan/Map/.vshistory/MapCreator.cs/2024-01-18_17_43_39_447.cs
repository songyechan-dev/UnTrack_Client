using GoogleSheetsToUnity.ThirdPary;
using System.Collections;
using System.Collections.Generic;
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
    private Vector3 startPosition = MapInfo.startPosition;
    private int round =1;

    [Header("Prefabs")]
    public GameObject planePrefab;
    public GameObject obPrefab;
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
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(this);
    }

    private IEnumerator CSVLoad()
    {
        // TODO : 구글 드라이브에서 가져와야 함 (수정 필요 2024.01.18 송예찬 - DataManager 이용)
        TextAsset mapCSV = Resources.Load<TextAsset>(MapInfo.mapDataCsvName + round.ToString());
        mapInfo.Clear();

        if (mapCSV != null)
        {
            string[] lines = mapCSV.text.Split('\n');
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
            isMapCSVLoaded = true; // 로딩이 완료되었음을 표시
        }
        else
        {
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
        mapParent.name = "MapParent";
        float originX = startPosition.x;

        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;

        float prevCreatedYPos;

        GameObject planeObject;
        GameObject obObject;
        GameObject trackObject;
        
        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                planeObject = Instantiate(planePrefab, mapParent.transform);
                planeObject.tag = "Plane";
                planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[j][i] == 1)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++)
                    {
                        obObject = Instantiate(obPrefab, mapParent.transform);
                        obObject.transform.position = new Vector3(x * objScale * 10, k == 0 ? planeObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10);
                        obObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                        prevCreatedYPos = obObject.transform.position.y;
                    }
                }
                else if (mapInfo[j][i] == 3 || mapInfo[j][i] == 4)
                {
                    trackObject = Instantiate(trackPrefab, mapParent.transform);
                    trackObject.AddComponent<TrackInfo>();
                    trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                    trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    trackObject.tag = "Track";
                    trackObject.transform.position = new Vector3(x * objScale * 10, trackPrefab.transform.localScale.y / 2, z * objScale * 10);
                    trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
                    if (mapInfo[j][i] == 3)
                    {
                        trackManager.finalTrack = trackObject;
                    }
                    else
                    {
                        Debug.Log("호출됨");
                        trackObject.GetComponent<TrackInfo>().isFinishedTrack = true;
                    }
                }
                z++;
            }
            x = originX;
            z++;
        }
        EditorUtility.SetDirty(mapParent.gameObject);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
