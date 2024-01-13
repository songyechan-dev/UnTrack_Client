using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
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

    private GameObject planPrefab;
    private GameObject obPrefab;
    private Transform mapParent;

    private List<List<int>> mapInfo = new List<List<int>>();

    private GameObject planObject = null;
    private GameObject obObject = null;
    private TextAsset mapCSV;



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
            if (GUILayout.Button("Map Data Create"))
            {
                Debug.Log("생성");
            }
        }
        else
        {
            objScale = EditorGUILayout.FloatField("맵 타일 사이즈 :", objScale);
            mapCSV = (TextAsset)EditorGUILayout.ObjectField("Map Data:", mapCSV, typeof(TextAsset), false);
            mapParent = (Transform)EditorGUILayout.ObjectField("맵 부모:", mapParent, typeof(Transform), true);
            planPrefab = (GameObject)EditorGUILayout.ObjectField("Plan Prefab:", planPrefab, typeof(GameObject), false);
            obPrefab = (GameObject)EditorGUILayout.ObjectField("OB Prefab:", obPrefab, typeof(GameObject), false);
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
                planObject = Instantiate(planPrefab, mapParent);
                planObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                planObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[i][j] == 1)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++) 
                    {
                        obObject = Instantiate(obPrefab, mapParent);
                        obObject.transform.position = new Vector3(x * objScale * 10, k == 0 ? planObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10); ;
                        obObject.transform.localScale = new Vector3(objScale * 10, objScale * 10, objScale * 10);
                        prevCreatedYPos = obObject.transform.position.y;
                    }
                }
                x++;
            }
            x = originX;
            z--;
        }
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


    }

}
