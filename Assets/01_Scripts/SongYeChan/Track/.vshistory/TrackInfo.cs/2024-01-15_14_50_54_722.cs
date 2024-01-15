using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrackInfo;

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
                transform.localEulerAngles = new Vector3(0,0,0);
                myDirection = MyDirection.UP;
                break;
            case MyDirection.DOWN:
                transform.localEulerAngles = new Vector3(0, 180f, 0);
                myDirection = MyDirection.DOWN;
                break;
            case MyDirection.LEFT:
                transform.localEulerAngles = new Vector3(0, -90f, 0);
                myDirection = MyDirection.LEFT;
                Debug.Log("변경됨 LEFT!");
                break;
            case MyDirection.RIGHT:
                transform.localEulerAngles = new Vector3(0, 90f, 0);
                myDirection = MyDirection.RIGHT;
                Debug.Log("변경됨 RIGHT");
                break;
            default:
                break;
        }
    }

    public MyDirection GetMyRotation()
    {
        switch ((transform.localRotation).eulerAngles.y)    
        {
            case 0:
                return MyDirection.UP;
            case 180f:
                return MyDirection.DOWN;
            case -90f:
                return MyDirection.LEFT;
            case 90f:
                return MyDirection.RIGHT;
            default:
                return 0;
        }
    }
}
