using GoogleSheetsToUnity.ThirdPary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    //TODO : startPosition -> stateManager 송예찬 2024.01.18
    SongYeChan.StateManager state;
    //임시로 만듬
    private void Create()
    {
        if (mapInfo.Count <= 0)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("오류!", "MapData를 Load 해주세요.", "OK");
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
                planObject.tag = "Plane";
                planObject.transform.position = new Vector3(x * objScale * 10, y, z * objScale * 10);
                planObject.transform.localScale = new Vector3(objScale, objScale, objScale);
                if (mapInfo[i][j] == 1)
                {
                    prevCreatedYPos = 0;
                    int yCount = Random.Range(1, 5);
                    for (int k = 0; k < yCount; k++)
                    {
                        obObject = Instantiate(obPrefab, mapParent);
                        obObject.transform.position = new Vector3(x * objScale * 10, k == 0 ? planObject.transform.position.y + objScale * 5 : (prevCreatedYPos + objScale * 10), z * objScale * 10);
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
