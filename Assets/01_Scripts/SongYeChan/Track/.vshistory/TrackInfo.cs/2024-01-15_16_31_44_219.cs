using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static TrackInfo;
[CustomPropertyDrawer(typeof(TrackInfo))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 예외 처리: property가 SerializableDictionary 타입이 아니면 에러 메시지를 표시하고 리턴
        if (property.type != typeof(TrackInfo).ToString())
        {
            EditorGUI.LabelField(position, "This is not a SerializableDictionary");
            EditorGUI.EndProperty();
            return;
        }

        // Dictionary를 찾아서 그려줌
        EditorGUI.PropertyField(position, property, true);

        EditorGUI.EndProperty();
    }
}
[System.Serializable]
public class TrackInfo : MonoBehaviour :Dictionary<ArroundTrackDirection, bool>
{
    public enum MyDirection
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    public enum ArroundTrackDirection
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    [Header("")]
    [SerializeField]
    public Transform leftTrack;
    [SerializeField]
    public Transform rightTrack;
    [SerializeField]
    public Transform upTrack;
    [SerializeField]
    public Transform downTrack;

    [SerializeField]
    public Dictionary<ArroundTrackDirection,bool> arroundTrackInfo = new Dictionary<ArroundTrackDirection,bool>();

    public MyDirection myDirection;
    public bool isElectricityFlowing = false;
    
    public float maxDistance = 1f;
    public string trackTagName = "Track";

    public void SetArroundTrackInfo(ArroundTrackDirection _arroundTrackDirection,bool _isElectricityFlowing)
    {
        arroundTrackInfo[_arroundTrackDirection] = _isElectricityFlowing;
    }

    public void SetMyDirection(MyDirection _myDirection)
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


    public void SetArroundTrack(Transform otherTrack, ArroundTrackDirection _arroundTrackDirection)
    {
        switch (_arroundTrackDirection)
        {
            case ArroundTrackDirection.UP:
                upTrack = otherTrack;
                break;
            case ArroundTrackDirection.DOWN:
                downTrack = otherTrack;
                break;
            case ArroundTrackDirection.LEFT:
                leftTrack = otherTrack;
                break;
            case ArroundTrackDirection.RIGHT:
                rightTrack = otherTrack;
                break;
            default:
                break;
        }
    }

}
