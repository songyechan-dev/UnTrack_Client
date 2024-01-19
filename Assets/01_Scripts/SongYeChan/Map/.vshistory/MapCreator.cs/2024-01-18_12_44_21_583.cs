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

    [Header("Prefabs")]
    public GameObject planePrefab;
    public GameObject obPrefab;

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

    private IEnumerator CSVLoad(int _round)
    {
        // TODO : 구글 드라이브에서 가져와야 함 (수정 필요 2024.01.18 송예찬 - DataManager 이용)
        TextAsset mapCSV = Resources.Load<TextAsset>(MapInfo.mapDataCsvName + _round.ToString());
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

    private IEnumerator WaitUntilMapCSVLoaded(int _round)
    {
        isMapCSVLoaded = false;
        yield return StartCoroutine(CSVLoad(_round));

        // CSV 로딩이 완료될 때까지 대기
        while (!isMapCSVLoaded)
        {
            yield return null;
        }

        Create(_round);
    }

    private void MapLoad(int _round)
    {
        StartCoroutine(WaitUntilMapCSVLoaded(_round));
    }

    private void Create(int _round)
    {
        if (mapInfo.Count <= 0)
        {
            return;
        }
        GameObject mapParent = Instantiate(new GameObject());
        float originX = startPosition.x;

        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;

        float prevCreatedYPos;
        GameObject planeObject;
        GameObject obObject;
        
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                planeObject = Instantiate(planePrefab, mapParent.transform);
                planeObject.tag = "Plane";
                planeObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                planeObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[i][j] == 1)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++)
                    {
                        obObject = Instantiate(obPrefab, mapParent);
                        obObject.transform.position = new Vector3(x * objScale * 10, k == 0 ? planeObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10);
                        obObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                        prevCreatedYPos = obObject.transform.position.y;
                    }
                }
                else if (mapInfo[i][j] == 3)
                {
                    trackObject = Instantiate(trackPrefab, mapParent);
                    trackObject.AddComponent<TrackInfo>();
                    trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                    trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    trackObject.tag = "Track";
                    trackObject.transform.position = new Vector3(x * objScale * 10, trackPrefab.transform.localScale.y / 2, z * objScale * 10);
                    trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
                    trackManager.finalTrack = trackObject;
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
