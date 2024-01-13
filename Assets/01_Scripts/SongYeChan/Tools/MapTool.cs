using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapTool : EditorWindow
{
    private int width;
    private int height;
    private float objScale;
    private Vector3 startPosition;

    private GameObject testPrefab;
    private Transform mapParent;

    private bool isRandomCreate;

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
        testPrefab = (GameObject)EditorGUILayout.ObjectField("Test Prefab:", testPrefab, typeof(GameObject), false);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);

        if (GUILayout.Button("Create") && width != 0 && height != 0 && !string.IsNullOrEmpty(width.ToString()))
        {
            TestCreate();
        }

        if (GUILayout.Button("Delete"))
        {
            TestDestroy();
        }
    }

    private void TestCreate()
    {
        float originX = startPosition.x;
        
        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject gameObject = Instantiate(testPrefab, mapParent);
                gameObject.transform.position = new Vector3(x * objScale *10,y,z * objScale * 10);
                gameObject.transform.localScale = new Vector3(objScale,objScale,objScale);
                x++;
            }
            x = originX;
            z++;
        }
    }

    private void TestDestroy()
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
