using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    enum Type
    {
        PLANE = 0,
        OBSTACLE = 1,
        PLAYER = 2,
        TRACK = 3,
        FiNISH_TRACK = 4,
        FACTORY = 5,
        STORAGE = 6,
        ENGINE = 7
    }
    public static string mapDataCsvName = "MapData";
    public static float objScale;
    public static Vector3 startPosition = Vector3.zero;
    public static Vector3 endPosition = Vector3.zero;
}
