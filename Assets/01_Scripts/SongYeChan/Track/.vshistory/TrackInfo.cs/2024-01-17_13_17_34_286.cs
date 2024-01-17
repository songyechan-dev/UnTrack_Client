using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TrackInfo;

public class TrackInfo : MonoBehaviour
{
    public enum MyDirection
    {
        FORWARD = 0,
        BACK = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    public enum ArroundTrackDirection
    {
        FORWARD = 0,
        BACK = 1,
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
    public GameObject factoriesObjectPrefab;

    public Vector3 prevAngle;

    public void SetArroundTrackInfo(ArroundTrackDirection _arroundTrackDirection,bool _isElectricityFlowing)
    {
        arroundTrackInfo[_arroundTrackDirection] = _isElectricityFlowing;
    }

    public void SetMyDirection(MyDirection _myDirection,Vector3 _angle)
    {
        switch (_myDirection)
        {
            case MyDirection.FORWARD:
                transform.localEulerAngles = _angle;
                myDirection = MyDirection.FORWARD;
                break;
            case MyDirection.BACK:
                transform.localEulerAngles = _angle;
                myDirection = MyDirection.BACK;
                break;
            case MyDirection.LEFT:
                transform.localEulerAngles = _angle;
                myDirection = MyDirection.LEFT;
                Debug.Log("변경됨 LEFT!");
                break;
            case MyDirection.RIGHT:
                transform.localEulerAngles = _angle;
                myDirection = MyDirection.RIGHT;
                Debug.Log("변경됨 RIGHT");
                break;
            default:
                break;
        }
    }

    public void GetOnFactoriesObject()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(factoriesObjectPrefab.tag))
            {
                Debug.Log("팩토리스오브젝트가 위에있음");
                if (hit.transform.localEulerAngles.y != transform.localEulerAngles.y)
                {
                    Debug.Log("회전값 다름");
                }
            }
        }


    }

    //TODO: 트러블 슈팅 - 각도 이슈(0,90,180,-90 각도가 정확히 나오지가 않아서 수정함)
    public MyDirection GetMyRotation()
    {
        float yRotation = transform.localRotation.eulerAngles.y;

        if (IsApproximately(yRotation, 0f))
        {
            return MyDirection.FORWARD;
        }
        else if (IsApproximately(yRotation, 180f))
        {
            return MyDirection.BACK;
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
            return MyDirection.FORWARD;
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
            case ArroundTrackDirection.FORWARD:
                upTrack = otherTrack;
                break;
            case ArroundTrackDirection.BACK:
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
