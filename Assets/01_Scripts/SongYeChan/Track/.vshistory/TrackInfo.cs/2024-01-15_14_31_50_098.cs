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
    public bool isElectricityFlowing = false;

    public void ChangeMyDirection(MyDirection myDirection)
    {
        switch (myDirection)
        {
            case MyDirection.UP:
                transform.rotation = (Quaternion.Euler(0,0,0));
                myDirection = MyDirection.UP;
                break;
            case MyDirection.DOWN:
                transform.rotation = (Quaternion.Euler(0, 180f, 0));
                break;
            case MyDirection.LEFT:
                transform.rotation = (Quaternion.Euler(0, -90f, 0));
                break;
            case MyDirection.RIGHT:
                transform.rotation = (Quaternion.Euler(0, 90f, 0));
                break;
            default:
                break;
        }
    }
}
