using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class MapTool : EditorWindow
{
    private int width;
    private int height;
    private int mapY;
    private int mapX;

    private float objScale;
    private Vector3 startPosition;

    private GameObject planPrefab;
    private GameObject obPrefab;
    private Transform mapParent;

    private List<List<int>> mapInfo = new List<List<int>>();

    private GameObject createdObject = null;

    [MenuItem("Tools/Map Tool")]
    public static void MapToolEditor()
    {
        GetWindow(typeof(MapTool), false, "MapTool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Tool", EditorStyles.boldLabel);
        width = EditorGUILayout.IntField("맵 전체 가로길이:", width);
        height = EditorGUILayout.IntField("맵 전체 세로길이:", height);
        objScale = EditorGUILayout.FloatField("맵 타일 사이즈 :", objScale);
        mapParent = (Transform)EditorGUILayout.ObjectField("맵 부모:", mapParent, typeof(Transform), true);
        planPrefab = (GameObject)EditorGUILayout.ObjectField("Plan Prefab:", planPrefab, typeof(GameObject), false);
        obPrefab = (GameObject)EditorGUILayout.ObjectField("OB Prefab:", obPrefab, typeof(GameObject), false);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);

        if (GUILayout.Button("MapShow") && mapInfo.Count > 0)
        {
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

    private void MapShow()
    {
        float originX = startPosition.x;
        
        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                createdObject = Instantiate(planPrefab, mapParent);
                createdObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                createdObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[i][j] == 1)
                {
                    createdObject = Instantiate(obPrefab, mapParent);
                    createdObject.transform.position = new Vector3(x * objScale * 10, y + 0.5f, z * objScale * 10);
                    createdObject.transform.localScale = new Vector3(objScale *10, objScale * 10, objScale * 10);
                }
                x++;
            }
            x = originX;
            z--;
        }
    }

    private void MapDestroy()
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

    private void CSVLoad()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MapData");
        mapInfo.Clear();
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
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
        }
        mapY = mapInfo.Count;
        mapX = mapInfo[0].Count;
    }

}
