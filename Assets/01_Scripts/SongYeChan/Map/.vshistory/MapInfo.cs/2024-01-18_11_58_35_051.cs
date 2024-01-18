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
        FACTORY = 4,
        STORAGE = 5,
        ENGINE = 6
    }
    public float objScale;
    public static Vector3 startPosition = Vector3.zero;
}
