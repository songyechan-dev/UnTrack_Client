using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackInfo : MonoBehaviour
{
    public enum MyDirection
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }
    public MyDirection myDirection;

    public void ChangeMyDirection(MyDirection myDirection)
    {
        switch (myDirection)
        {
            case MyDirection.UP:
                transform.rotation = (Quaternion.Euler(0,0,0));
                break;
            case MyDirection.DOWN:
                break;
            case MyDirection.LEFT:
                transform.rotation = (Quaternion.Euler(0, 90f, 0));
                break;
            case MyDirection.RIGHT:
                break;
            default:
                break;
        }
    }
}
