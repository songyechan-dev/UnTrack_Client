using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    enum Type
    {
        PLANE = 0,
        OBSTACLE_STONE = 1,
        OBSTACLE_TREE = 2,
        TRACK = 3,
        FiNISH_TRACK = 4,
        FACTORY = 5,
        STORAGE = 6,
        ENGINE = 7
    }
    public static string mapDataCsvName = "MapData";
    public static float objScale = 0.1f;
    public static Vector3 startPosition = Vector3.zero;
    public static Vector3 endPosition = Vector3.zero;
    public static Vector3 startTrackRotation;
    public static Vector3 endTrackRotation;
    public static string trackYRotationInfoFileName = "TrackYRotationInfo";
    public static string startTrackYRotationKeyName = "startTrackYRotation";
    public static string endTrackYRotationKeyName = "endTrackYRotation";

    public static int mapWidth = 50;
    public static int mapHeight = 20;
    public static float startTrackYRotation = 90f;
    public static float endTrackYRotation = 90f;

    public static int defaultStartTrackZ;
    public static int defaultStartTrackX;

    public static int defaultEndTrackZ;
    public static int defaultEndTrackX;

    public static int finishStartTrackZ;
    public static int finishStartTrackX;

    public static int finishEndTrackZ;
    public static int finishEndTrackX;

}
