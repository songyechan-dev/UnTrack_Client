using Google.Protobuf.WellKnownTypes;
using GoogleSheetsToUnity.ThirdPary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapTool : EditorWindow
{
    public enum MapMode
    {
        ShowMap,
        CreateMap
    }
    private MapMode selectedMode;


    private int width;
    private int height;
    private int mapY;
    private int mapX;

    private float prevCreatedYPos;

    private float objScale;
    private Vector3 startPosition;

    private GameObject planePrefab;
    private GameObject obPrefab;
    private GameObject trackPrefab;

    private Transform mapParent;

    private List<List<int>> mapInfo = new List<List<int>>();

    private GameObject planeObject = null;
    private GameObject obObject = null;
    private GameObject trackObject = null;
    private GameObject factoriesObjectPrefab = null;
    private TextAsset mapCSV;
    private TextAsset startTrackYRotationInfoCSV;

    private Dictionary<int, int> rotationInfoDict = new Dictionary<int, int>();

    private int defaultStartTrackZ;
    private int defaultStartTrackX;

    private int defaultEndTrackZ;
    private int defaultEndTrackX;

    private int finishStartTrackZ;
    private int finishStartTrackX;

    private int finishEndTrackZ;
    private int finishEndTrackX;

    private float startTrackYRotation;
    private float endTrackYRotation;

    private float startTrackYRotation_CSV;
    private float endTrackYRotation_CSV;

    private TrackManager trackManager;

    private string csvFileName;
    private string rotationInfoFileName;

    [MenuItem("Tools/Map Tool")]
    public static void MapToolEditor()
    {
        GetWindow(typeof(MapTool), false, "MapTool");
    }

    private void OnGUI()
    {
        selectedMode = (MapMode)EditorGUILayout.EnumPopup("Select Map Mode:", selectedMode);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);
        if (selectedMode.Equals(MapMode.CreateMap))
        {
            width = EditorGUILayout.IntField("맵 전체 가로길이:", width);
            height = EditorGUILayout.IntField("맵 전체 세로길이:", height);
            EditorGUILayout.Space();
            defaultStartTrackZ = EditorGUILayout.IntField("Default Track Start Z:", defaultStartTrackZ);
            defaultStartTrackX = EditorGUILayout.IntField("Default Track Start X:", defaultStartTrackX);

            defaultEndTrackZ = EditorGUILayout.IntField("Default Track End Z:", defaultEndTrackZ);
            defaultEndTrackX = EditorGUILayout.IntField("Default Track End X:", defaultEndTrackX);
            EditorGUILayout.Space();
            finishStartTrackZ = EditorGUILayout.IntField("Finish Track Start Z:", finishStartTrackZ);
            finishStartTrackX = EditorGUILayout.IntField("Finish Track Start X:", finishStartTrackX);

            finishEndTrackZ = EditorGUILayout.IntField("Finish Track End Z:", finishEndTrackZ);
            finishEndTrackX = EditorGUILayout.IntField("Finish Track End X:", finishEndTrackX);
            EditorGUILayout.Space();
            startTrackYRotation = EditorGUILayout.FloatField("Start TrackY Rotation:", startTrackYRotation);
            endTrackYRotation = EditorGUILayout.FloatField("End TrackY Rotation:", endTrackYRotation);
            EditorGUILayout.Space();
            csvFileName = EditorGUILayout.TextField("MapData FileName", csvFileName);
            rotationInfoFileName = EditorGUILayout.TextField("rotationInfoFileName", rotationInfoFileName);
            if (GUILayout.Button("Map Data Create"))
            {
                MapDataCreate();
            }
        }
        else
        {
            trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
            objScale = EditorGUILayout.FloatField("맵 타일 사이즈 :", objScale);
            mapCSV = (TextAsset)EditorGUILayout.ObjectField("Map Data:", mapCSV, typeof(TextAsset), false);
            startTrackYRotationInfoCSV = (TextAsset)EditorGUILayout.ObjectField("startTrackYRotationInfoCSV:", startTrackYRotationInfoCSV, typeof(TextAsset), false);
            mapParent = (Transform)EditorGUILayout.ObjectField("맵 부모:", mapParent, typeof(Transform), true);
            planePrefab = (GameObject)EditorGUILayout.ObjectField("Plan Prefab:", planePrefab, typeof(GameObject), false);
            obPrefab = (GameObject)EditorGUILayout.ObjectField("OB Prefab:", obPrefab, typeof(GameObject), false);
            trackPrefab = (GameObject)EditorGUILayout.ObjectField("Track Prefab:", trackPrefab, typeof(GameObject), false);
            factoriesObjectPrefab = (GameObject)EditorGUILayout.ObjectField("factoriesObjectPrefab:", factoriesObjectPrefab, typeof(GameObject), false);
            if (GUILayout.Button("MapShow"))
            {
                MapDestroy();
                MapShow();
            }

            if (GUILayout.Button("Delete"))
            {
                MapDestroy();
            }

            if (GUILayout.Button("TestCSVLoad"))
            {
                CSVLoad();
            }
        }
    }

    private void MapShow()
    {
        if (mapInfo.Count <= 0)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("오류!","MapData를 Load 해주세요.","OK");
#endif
            return;
        }
        float originX = startPosition.x;
        
        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                planeObject = Instantiate(planePrefab, mapParent);
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
                else if (mapInfo[i][j] == 3 || mapInfo[i][j] == 4)
                {
                    trackObject = Instantiate(trackPrefab, mapParent);
                    trackObject.AddComponent<TrackInfo>();
                    trackObject.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                    trackObject.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    trackObject.tag = "Track";
                    trackObject.transform.position = new Vector3(x * objScale * 10, trackPrefab.transform.localScale.y / 2, z * objScale * 10);
                    trackObject.transform.localScale = new Vector3(objScale * 10, trackPrefab.transform.localScale.y, objScale * 10);
                    if (mapInfo[i][j] == 3)
                    {
                        trackObject.transform.localEulerAngles= new Vector3(0,startTrackYRotation,0);
                        trackManager.finalTrack = trackObject;
                    }
                    else
                    {
                        Debug.Log("호출됨");
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

    private void MapDestroy()
    {
        if (mapParent != null)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in mapParent)
            {
                children.Add(child);
            }
            foreach (Transform child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void CSVLoad()
    {
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
        }
        else
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("오류!", "MapData 파일을 불러와 주세요.", "OK");
#endif
            return;
        }

        if (startTrackYRotationInfoCSV != null)
        {
            string[] lines = startTrackYRotationInfoCSV.text.Split('\n');
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                if (values.Length >= 2)
                {
                    int startRotation = int.Parse(values[0]);
                    int endRotation = int.Parse(values[1]);
                    rotationInfoDict.Add(startRotation, endRotation);
                }
                else
                {
                    Debug.LogError("Invalid CSV format: " + line);
                }
            }
        }
        else
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("오류!", "MapData 파일을 불러와 주세요.", "OK");
#endif
            return;
        }
    }

    private void MapDataCreate()
    {
        string mapDataFilePath = Path.Combine(Application.streamingAssetsPath, csvFileName);
        string startTrackRotationInfoFilePath = Path.Combine(Application.streamingAssetsPath, rotationInfoFileName);
        string content = "";
        string rotationInfo = $"startTrackYRotation,{startTrackYRotation}\nendTrackYRotation,{endTrackYRotation}";
        string mapInfo = "0";

        int randomXcount = 0;
        int originX = 0;
        int originY = 0;
        int randomYcount = 0;
        bool isCreatedOb = false;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapInfo = "0";
                // TODO : 랜덤 범위 지정 필요(2024.01.14) - 송예찬 MapTool.cs
                if (!isCreatedOb)
                {
                    int randomCount = Random.Range(0, 50);
                    randomXcount = Random.Range(3, 7);
                    randomYcount = Random.Range(3, 7);
                    if (randomCount < 1)
                    {
                        isCreatedOb = true;
                        originX = x;
                        originY = y;
                    }
                }

                if (isCreatedOb)
                {
                    if (y > (randomYcount + originY))
                    {
                        randomXcount = 0;
                        randomYcount = 0;
                        originX = 0;
                        originY = 0;
                        isCreatedOb = false;
                    }
                    else if (x <= (randomXcount + originX) && x >= originX)
                    {
                        mapInfo = "1";
                    }
                }
                //출발Track
                if ((x >= defaultStartTrackX && x <= defaultEndTrackX) && (y >= defaultStartTrackZ && y <= defaultEndTrackZ))
                {
                    mapInfo = "3";
                }

                //도착Track
                if ((x >= finishStartTrackX && x <= finishEndTrackX) && (y >= finishStartTrackZ && y <= finishEndTrackZ))
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
            if (y < height - 1)
            {
                content += "\n";
            }
        }

        File.WriteAllText(mapDataFilePath, content);
        File.WriteAllText(startTrackRotationInfoFilePath, rotationInfo);
        AssetDatabase.Refresh();
    }

}

