using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    public enum Type
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

    public static int mapWidth = 100;
    public static int mapHeight = 20;
    public static float startTrackYRotation = 90f;
    public static float endTrackYRotation = 90f;

    public static int defaultStartTrackZ = 5;
    public static int defaultStartTrackX = 3;

    public static int defaultEndTrackZ = 5;
    public static int defaultEndTrackX = 8;

    public static int finishStartTrackZ = 5;
    public static int finishStartTrackX = 40;

    public static int finishEndTrackZ = 5;
    public static int finishEndTrackX = 42;

    public static float trackYscale = 0.2f;

}
