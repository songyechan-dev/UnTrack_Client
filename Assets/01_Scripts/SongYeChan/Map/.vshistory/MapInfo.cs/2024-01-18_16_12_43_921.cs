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
        FiNISH_TRACK = 3.1,
        FACTORY = 4,
        STORAGE = 5,
        ENGINE = 6
    }
    public static string mapDataCsvName = "MapData";
    public static float objScale;
    public static Vector3 startPosition = Vector3.zero;
    public static Vector3 endPosition = Vector3.zero;
}
