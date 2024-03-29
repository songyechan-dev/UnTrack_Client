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

    public float rotationSpeed = 10f;
    public float rotationPerFrame = 1.5f;

    MoveDirection changedRotateAngle;
    public bool isStop = false;
    public bool isChangedRotation = false;
    Vector3 direction;
    Transform sensorTransform;

    RaycastHit hit;

    private float curTime = 0f;
    // 시작
    void Start()
    {
        SetMoveDirection();
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
        if (moveDirection == MoveDirection.FORWARD)
        {
            if (isChangedRotation)
            {
                float lessAngle = changedRotateAngle.Equals(MoveDirection.LEFT) ? -90f : 270f;
                float greaterAngle = changedRotateAngle.Equals(MoveDirection.LEFT) ? 90f : -270f;
                float np = changedRotateAngle.Equals(MoveDirection.LEFT) ? -1 : 1;
                transform.Translate(Vector3.forward * 0.2f * Time.deltaTime);
                transform.Rotate((new Vector3(0, rotationPerFrame * np, 0)) * rotationSpeed * Time.deltaTime);
                Debug.Log(lessAngle);
                Debug.Log(greaterAngle);
                if (transform.rotation.eulerAngles.y <= lessAngle || transform.rotation.eulerAngles.y < greaterAngle)
                {
                    Debug.Log("끝남");
                }
            }
            else
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            
        }
        else if ((moveDirection.Equals(MoveDirection.LEFT) || moveDirection.Equals(MoveDirection.RIGHT)) && !isStop)
        {
            float lessAngle = moveDirection.Equals(MoveDirection.LEFT) ? -90f : 270f;
            float greaterAngle = moveDirection.Equals(MoveDirection.LEFT) ? 90f : -270f;
            float np = moveDirection.Equals(MoveDirection.LEFT) ? -1 : 1;
            isChangedRotation = true;
            changedRotateAngle = moveDirection.Equals(MoveDirection.LEFT) ? MoveDirection.LEFT : MoveDirection.RIGHT;
            transform.Translate(Vector3.forward * 0.2f * Time.deltaTime);
            transform.Rotate((new Vector3(0, rotationPerFrame * np, 0)) * rotationSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y <= lessAngle || transform.rotation.eulerAngles.y < greaterAngle)
            {
                Debug.Log("끝남");
            }
        }
        else if (moveDirection.Equals(MoveDirection.RIGHT))
        {
            moveDirection = MoveDirection.FORWARD;
        }
    }


    IEnumerator SetIsStop()
    {
        yield return null;
    }

    void WaitToFire()
    {
        curTime += Time.deltaTime;
        if (curTime > fireTime)
        {
            //Debug.Log("파이어!!!");
        }
    }

    void SetMoveDirection()
    {
        switch (moveDirection)
        {
            case MoveDirection.FORWARD:
                direction = Vector3.forward;
                break;
            case MoveDirection.BACK:
                direction = Vector3.back;
                break;
            case MoveDirection.LEFT:
                direction = Vector3.left;
                break;
            case MoveDirection.RIGHT:
                direction = Vector3.right;
                break;
            case MoveDirection.UP:
                direction = Vector3.up;
                break;
            case MoveDirection.DOWN:
                direction = Vector3.down;
                break;
            default:
                break;
        }
    }



}
