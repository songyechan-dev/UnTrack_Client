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

    public enum AroundTrackDirection
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    public MyDirection myDirection;
    public bool isElectricityFlowing = false;
    public Transform leftTrack;
    public Transform rightTrack;
    public Transform upTrack;
    public Transform downTrack;
    public float maxDistance = 1f;
    public string trackTagName = "Track";

    public void ChangeMyDirection(MyDirection _myDirection)
    {
        switch (_myDirection)
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

    //TODO: 트러블 슈팅 - 각도 이슈(0,90,180,-90 각도가 정확히 나오지가 않아서 수정함)
    public MyDirection GetMyRotation()
    {
        float yRotation = transform.localRotation.eulerAngles.y;

        if (IsApproximately(yRotation, 0f))
        {
            return MyDirection.UP;
        }
        else if (IsApproximately(yRotation, 180f))
        {
            return MyDirection.DOWN;
        }
        else if (IsApproximately(yRotation, -90f) || IsApproximately(yRotation, 270f))
        {
            return MyDirection.LEFT;
        }
        else if (IsApproximately(yRotation, 90f) || IsApproximately(yRotation, -270f))
        {
            return MyDirection.RIGHT;
        }
        else
        {
            return MyDirection.UP;
        }

        // 각도를 비교할 때 사용할 정밀도를 설정하는 함수
        bool IsApproximately(float a, float b, float epsilon = 0.01f)
        {
            return Mathf.Abs(a - b) < epsilon;
        }
    }


    public void SetAroundTrack(Transform otherTrack, AroundTrackDirection _aroundTrackDirection)
    {
        switch (_aroundTrackDirection)
        {
            case AroundTrackDirection.UP:
                upTrack = otherTrack;
                break;
            case AroundTrackDirection.DOWN:
                downTrack = otherTrack;
                break;
            case AroundTrackDirection.LEFT:
                leftTrack = otherTrack;
                break;
            case AroundTrackDirection.RIGHT:
                break;
            default:
                break;
        }
    }

}
