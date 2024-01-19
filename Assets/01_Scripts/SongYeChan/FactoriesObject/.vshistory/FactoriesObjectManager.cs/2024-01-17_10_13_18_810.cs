using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FactoriesObjectManager : MonoBehaviour
{
    enum MyState 
    {
        STOP = 0,
        MOVE = 1,
        FIRE = 2
    }
    enum MoveDirection
    {
        FORWARD = 0,
        BACK =1,
        LEFT=2,
        RIGHT=3,
        UP =4,
        DOWN = 5,
    }
    MyState myState = MyState.STOP;

    [SerializeField]
    string tagToBeDetected;
    [SerializeField]
    MoveDirection moveDirection;
    [SerializeField, Range(0f, 100f)]
    public float moveSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    public float fireTime;
    [SerializeField, Range(0f, 100f)]
    public float rotationSpeed = 10f;
    [SerializeField, Range(0f, 500f)]
    public float rotationPerFrame = 1.5f;
    [SerializeField, Range(0f, 100f)]
    public float rotationForwardSpeed = 0.1f;

    float originMoveSpeed = -1f;
    public bool isStop = false;
    public bool isChangedRotation = false;

    private bool isChangedOriginMoveSpeed = false;

    MoveDirection changedRotateAngle;
    Transform sensorTransform;
    RaycastHit hit;
    Transform targetTransform;

    RaycastHit setLockedRayCastHit;

    private float curTime = 0f;
    // 시작
    void Start()
    {
        float num = 0.15f;
        for (int i = 1; i < 16; i++)
        {
            Debug.Log(num +=0.33f);
        }
        
        myState = MyState.STOP;
        sensorTransform = transform.Find("Sensor");
    }
    // 업데이트
    void Update()
    {
        if (!isStop)
        {
            SensorDetect();
        }
    }
    /// <summary>
    /// 센서 감지를 통해 센서 Ray에 감지된 Track의 속성값으로 이동하는 방향 변경하는 함수
    /// </summary>
    void SensorDetect()
    {
        if (sensorTransform != null)
        {
            if (isStop)
            {
                return;
            }
            Ray ray = new Ray(sensorTransform.position, -sensorTransform.up);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag(tagToBeDetected))
                {
                    if (hit.transform.GetComponent<TrackInfo>().myDirection == TrackInfo.MyDirection.LEFT)
                    {
                        moveDirection = MoveDirection.LEFT;
                    }
                    else if (hit.transform.GetComponent<TrackInfo>().myDirection == TrackInfo.MyDirection.RIGHT)
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
                    if (!isChangedRotation)
                    {
                        myState = MyState.STOP;
                    }
                    
                }
            }
            else
            {
                if (!isChangedRotation)
                {
                    myState = MyState.STOP;
                }
            }
        }
        else
        {
            Debug.LogError("Sensor 오브젝트없음");

        }
        Move();
    }

    void Move()
    {
        if (myState.Equals(MyState.MOVE))
        {
            RotationToDirection();
        }
        else
        {
            WaitToFire();
        }
    }

    /// <summary>
    /// SensorDetect() 함수를 통해 구해진 방향으로 회전
    /// </summary>
    void RotationToDirection()
    {
        curTime = 0; //TODO : 초기화 시킬건지 상의 필요(2024.01.14) - 송예찬 FactoriesObjectManager.cs
        if (isChangedRotation)
        {
            float np = changedRotateAngle.Equals(MoveDirection.LEFT) ? -1 : 1;
            float finishedAngle = 90 * np;
            Debug.Log(transform.position.z + " ," + setLockedRayCastHit.transform.position.z);
            if ((int)transform.position.z == (int)setLockedRayCastHit.transform.position.z)
            {
                rotationForwardSpeed = 0f;
            }
            transform.Translate(Vector3.forward * (rotationForwardSpeed) * Time.deltaTime);
            transform.Rotate((new Vector3(0, rotationPerFrame * np, 0)) * rotationSpeed * Time.deltaTime);
            if (!isChangedOriginMoveSpeed)
            {
                SetOriginMoveSpeed();
            }
            isChangedOriginMoveSpeed = true;
            moveSpeed = 0f;
            Debug.Log(transform.rotation.eulerAngles.y);
            if (np < 0)
            {
                if (transform.rotation.eulerAngles.y <= -90f || transform.rotation.eulerAngles.y < 270f)
                {
                    TurnFinsh(finishedAngle);
                }
            }
            else if (np > 0)
            {
                if (transform.rotation.eulerAngles.y >0 && transform.rotation.eulerAngles.y >= 90)
                {
                    TurnFinsh(finishedAngle);
                }
            }
            
        }
        else if (moveDirection == MoveDirection.FORWARD)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        else if ((moveDirection.Equals(MoveDirection.LEFT) || moveDirection.Equals(MoveDirection.RIGHT)) && !isStop)
        {
            isChangedRotation = true;
            changedRotateAngle = moveDirection.Equals(MoveDirection.LEFT) ? MoveDirection.LEFT : MoveDirection.RIGHT;
            RaycastHit lockedPos;
            if (Physics.Raycast(sensorTransform.position, Vector3.down, out lockedPos))
            {
                if (lockedPos.transform.CompareTag(tagToBeDetected))
                {
                    setLockedRayCastHit = lockedPos;
                }
                else
                {
                    setLockedRayCastHit = new RaycastHit();
                }
            }

        }
        else if (moveDirection.Equals(MoveDirection.RIGHT))
        {
            moveDirection = MoveDirection.FORWARD;
        }
    }

    void SetOriginMoveSpeed()
    {
        originMoveSpeed = moveSpeed;
    }

    /// <summary>
    /// RotationToDirection() 함수를 통해 회전후 회전이 끝나면 position 보정
    /// </summary>
    void TurnFinsh(float _finishedAngle)
    {
        //RaycastHit lockedHit;
        //if (Physics.Raycast(transform.position, Vector3.down, out lockedHit))
        //{
        //    if (lockedHit.transform.CompareTag(tagToBeDetected))
        //    {
        //        targetTransform = lockedHit.transform;
        //    }
        //}
        //else
        //{
        //    targetTransform = hit.transform;
        //}
        //transform.position = targetTransform.position + new Vector3(0, targetTransform.localScale.y / 2 + transform.localScale.y / 2, 0);
        transform.rotation = Quaternion.Euler(0, _finishedAngle, 0);
        isChangedRotation = false;
        Debug.Log(originMoveSpeed);
        moveSpeed = originMoveSpeed;
        originMoveSpeed = -1f;
        rotationForwardSpeed = 1f;
        hit = new RaycastHit();
        targetTransform = null;
        isChangedOriginMoveSpeed = false;
    }

    void WaitToFire()
    {
        curTime += Time.deltaTime;
        if (curTime > fireTime)
        {
            //Debug.Log("파이어!!!");
        }
    }

    void Turn(Transform _transform)
    {
        
    }



}
