using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, Range(0f, 100f)]
    public float rotationPerFrame = 1.5f;

    MoveDirection changedRotateAngle;
    public bool isStop = false;
    public bool isChangedRotation = false;
    Transform sensorTransform;
    RaycastHit hit;


    private float curTime = 0f;
    // 시작
    void Start()
    {
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
    /// 센서 감지를 통해 앞에있는 Track의 속성값 가져오는 함수
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
            Debug.LogError("Sensor 오브젝트없음");
        }
        Move();
    }

    void Move()
    {
        if (myState.Equals(MyState.MOVE))
        {
            CheckDirection();
        }
        else
        {
            WaitToFire();
        }
    }

    void CheckDirection()
    {
        curTime = 0; //TODO : 초기화 시킬건지 상의 필요(2024.01.14) - 송예찬 FactoriesObjectManager.cs
        if (isChangedRotation)
        {
            float np = changedRotateAngle.Equals(MoveDirection.LEFT) ? -1 : 1;
            transform.Translate(Vector3.forward * 0.2f * Time.deltaTime);
            transform.Rotate((new Vector3(0, rotationPerFrame * np, 0)) * rotationSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y <= 90f  * np || transform.rotation.eulerAngles.y < 270f * -np)
            {
                TurnFinsh();
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
        }
        else if (moveDirection.Equals(MoveDirection.RIGHT))
        {
            moveDirection = MoveDirection.FORWARD;
        }
    }

    void TurnFinsh()
    {
        Transform _targetTransform = hit.transform;
        isChangedRotation = false;
        transform.position = _targetTransform.position + new Vector3(0, _targetTransform.localScale.y /2 + transform.localScale.y /2, 0);
        hit = new RaycastHit();
    }

    void WaitToFire()
    {
        curTime += Time.deltaTime;
        if (curTime > fireTime)
        {
            //Debug.Log("파이어!!!");
        }
    }



}
