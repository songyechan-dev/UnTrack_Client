using System.Collections;
using UnityEngine;

public class FactoriesObjectManager : MonoBehaviour
{
    // 상태 열거형
    enum MyState
    {
        STOP = 0,
        MOVE = 1,
        FIRE = 2
    }

    // 이동 방향 열거형
    enum MoveDirection
    {
        FORWARD = 0,
        BACK = 1,
        LEFT = 2,
        RIGHT = 3,
        UP = 4,
        DOWN = 5,
    }

    // 멤버 변수
    MyState myState = MyState.STOP;
    MoveDirection moveDirection;
    bool isStop = false;

    [SerializeField] string tagToBeDetected;
    [SerializeField, Range(0f, 100f)] float moveSpeed = 10f;
    [SerializeField, Range(0f, 100f)] float fireTime;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float rotationPerFrame = 1.5f;
    [SerializeField] float changedRotateAngle;
    [SerializeField] Transform sensorTransform; // 센서 오브젝트의 Transform을 직접 인스펙터에서 설정

    float curTime = 0f;

    // 시작 메서드
    void Start()
    {
        SetMoveDirection();
        myState = MyState.STOP;
    }

    // 업데이트 메서드
    void Update()
    {
        if (!isStop)
        {
            SensorDetect();
        }
    }

    // 센서 감지 메서드
    void SensorDetect()
    {
        if (sensorTransform != null)
        {
            Ray ray = new Ray(sensorTransform.position, -sensorTransform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag(tagToBeDetected))
                {
                    TrackInfo.MyDirection trackDirection = hit.transform.GetComponent<TrackInfo>().GetMyRotation();

                    if (trackDirection == TrackInfo.MyDirection.LEFT)
                    {
                        moveDirection = MoveDirection.LEFT;
                    }
                    else if (trackDirection == TrackInfo.MyDirection.RIGHT)
                    {
                        moveDirection = MoveDirection.RIGHT;
                    }
                    else
                    {
                        moveDirection = MoveDirection.FORWARD;
                    }

                    myState = MyState.MOVE;
                }
                else
                {
                    myState = MyState.STOP;
                }
            }
            else
            {
                myState = MyState.STOP;
            }
        }
        else
        {
            Debug.LogError("Sensor 오브젝트 없음");
        }

        Move();
    }

    // 이동 메서드
    void Move()
    {
        if (myState == MyState.MOVE)
        {
            CheckDirection();
        }
        else
        {
            WaitToFire();
        }
    }

    // 이동 방향 확인 메서드
    void CheckDirection()
    {
        curTime = 0;

        if (moveDirection == MoveDirection.FORWARD)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (moveDirection == MoveDirection.LEFT && !isStop)
        {
            changedRotateAngle -= rotationPerFrame;
            transform.Translate(Vector3.forward * 0.2f * Time.deltaTime);
            transform.Rotate(new Vector3(0, -rotationPerFrame, 0) * rotationSpeed * Time.deltaTime);
            Debug.Log(transform.rotation.eulerAngles.y);

            float normalizedAngle = Mathf.Repeat(transform.eulerAngles.y, 360f);

            if (normalizedAngle >= 270f && normalizedAngle <= 450f)
            {
                moveDirection = MoveDirection.FORWARD;
                myState = MyState.STOP;
                isStop = true;
                StartCoroutine(SetIsStop());
                return;
            }
        }
        else if (moveDirection == MoveDirection.RIGHT)
        {
            moveDirection = MoveDirection.FORWARD;
        }
    }

    // 정지 설정 코루틴
    IEnumerator SetIsStop()
    {
        yield return new WaitForSeconds(0.5f);
        moveDirection = MoveDirection.FORWARD;
        myState = MyState.MOVE;
        isStop = false;
    }

    // 발사 대기 메서드
    void WaitToFire()
    {
        curTime += Time.deltaTime;
        if (curTime > fireTime)
        {
            // Debug.Log("파이어!!!");
        }
    }

    // 이동 방향 설정 메서드
    void SetMoveDirection()
    {
        switch (moveDirection)
        {
            case MoveDirection.FORWARD:
                transform.forward = Vector3.forward;
                break;
            case MoveDirection.BACK:
                transform.forward = Vector3.back;
                break;
            case MoveDirection.LEFT:
                transform.forward = Vector3.left;
                break;
            case MoveDirection.RIGHT:
                transform.forward = Vector3.right;
                break;
            case MoveDirection.UP:
                transform.forward = Vector3.up;
                break;
            case MoveDirection.DOWN:
                transform.forward = Vector3.down;
                break;
            default:
                break;
        }
    }
}
