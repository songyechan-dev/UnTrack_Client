using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TrackInfo;

public class TrackInfo : MonoBehaviour
{
    public enum ShapeDirection
    {
        FORWARD = 0,
        BACK = 1,
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
    public Transform forwardTrack;
    [SerializeField]
    public Transform backTrack;

    [SerializeField]
    public Dictionary<ArroundTrackDirection,bool> arroundTrackInfo = new Dictionary<ArroundTrackDirection,bool>();

    public ShapeDirection myDirection;
    public bool isElectricityFlowing = false;
    
    public float maxDistance = 1f;
    public string trackTagName = "Track";

    public void SetArroundTrackInfo(ArroundTrackDirection _arroundTrackDirection,bool _isElectricityFlowing)
    {
        arroundTrackInfo[_arroundTrackDirection] = _isElectricityFlowing;
    }

    public void SetMyDirection(ShapeDirection _shapeDirection)
    {
        switch (_shapeDirection)
        {
            case ShapeDirection.FORWARD:
                transform.localEulerAngles = new Vector3(0,0,0);
                myDirection = ShapeDirection.FORWARD;
                break;
            case ShapeDirection.BACK:
                transform.localEulerAngles = new Vector3(0, 180f, 0);
                myDirection = ShapeDirection.BACK;
                break;
            case ShapeDirection.LEFT:
                transform.localEulerAngles = new Vector3(0, -90f, 0);
                myDirection = ShapeDirection.LEFT;
                Debug.Log("변경됨 LEFT!");
                break;
            case ShapeDirection.RIGHT:
                transform.localEulerAngles = new Vector3(0, 90f, 0);
                myDirection = ShapeDirection.RIGHT;
                Debug.Log("변경됨 RIGHT");
                break;
            default:
                break;
        }
    }

    //TODO: 트러블 슈팅 - 각도 이슈(0,90,180,-90 각도가 정확히 나오지가 않아서 수정함)
    public ShapeDirection GetMyRotation()
    {
        float yRotation = transform.localRotation.eulerAngles.y;

        if (IsApproximately(yRotation, 0f))
        {
            return ShapeDirection.FORWARD;
        }
        else if (IsApproximately(yRotation, 180f))
        {
            return ShapeDirection.BACK;
        }
        else if (IsApproximately(yRotation, -90f) || IsApproximately(yRotation, 270f))
        {
            return ShapeDirection.LEFT;
        }
        else if (IsApproximately(yRotation, 90f) || IsApproximately(yRotation, -270f))
        {
            return ShapeDirection.RIGHT;
        }
        else
        {
            return ShapeDirection.FORWARD;
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
                forwardTrack = otherTrack;
                break;
            case ArroundTrackDirection.DOWN:
                backTrack = otherTrack;
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


    private Material material;
    private float offset = 0.0f;
    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
    }

    private void Update()
    {
        offset += 0.5f * Time.deltaTime;
        if (offset <= 0f)
        {
            material.mainTextureOffset = new Vector2(0, offset);
        }
        else if (offset >= 1.5f)
        {
            offset -= 1.5f;
            material.mainTextureOffset = new Vector2(0, offset);
        }
        else
        {
            material.mainTextureOffset = new Vector2(0, offset);
        }
    }

}
