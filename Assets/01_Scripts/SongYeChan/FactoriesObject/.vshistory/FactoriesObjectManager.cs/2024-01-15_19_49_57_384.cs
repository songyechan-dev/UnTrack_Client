using System.Collections;
using System.Collections.Generic;
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
        FORWORD = 0,
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

    public float changedRotateAngle;
    public bool isStop = false;

    Vector3 direction;

    private float curTime = 0f;
    // 시작
    void Start()
    {
        SetMoveDirection();
        myState = MyState.STOP;
    }

    // 업데이트
    void Update()
    {
        SensorDetect();
    }

    void SensorDetect()
    {
        Transform sensorTransform = transform.Find("Sensor");
        if (sensorTransform != null)
        {
            Ray ray = new Ray(sensorTransform.position, -sensorTransform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag(tagToBeDetected))
                {
                    if (hit.transform.GetComponent<TrackInfo>().GetMyRotation().Equals(TrackInfo.MyDirection.LEFT))
                    {
                        moveDirection = MoveDirection.LEFT;
                    }
                    else if (hit.transform.GetComponent<TrackInfo>().GetMyRotation().Equals(TrackInfo.MyDirection.RIGHT))
                    {
                        moveDirection = MoveDirection.RIGHT;
                    }
                    else
                    {
                        moveDirection = MoveDirection.FORWORD;
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
            curTime = 0; //TODO : 초기화 시킬건지 상의 필요(2024.01.14) - 송예찬 FactoriesObjectManager.cs
            if (moveDirection == MoveDirection.FORWORD)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if (moveDirection.Equals(MoveDirection.LEFT) && !isStop)
            {
                changedRotateAngle -= rotationPerFrame;
                transform.Translate(Vector3.forward * 0.2f * Time.deltaTime);
                transform.Rotate((new Vector3(0, -rotationPerFrame, 0)) * rotationSpeed * Time.deltaTime);
                Debug.Log(transform.rotation.eulerAngles.y);
                if (transform.rotation.eulerAngles.y <= -90f || transform.rotation.eulerAngles.y < 270f)
                {
                    moveDirection = MoveDirection.FORWORD;
                    myState = MyState.STOP;
                    isStop = true;
                    StartCoroutine(SetIsStop());
                    return;
                }
            }
            else if (moveDirection.Equals(MoveDirection.RIGHT))
            {

                moveDirection = MoveDirection.FORWORD;
            }
        }
        else
        {
            WaitToFire();
        }
    }
    IEnumerator SetIsStop()
    {
        yield return new WaitForSeconds(0.5f);
        moveDirection = MoveDirection.FORWORD;
        myState = MyState.MOVE;
        isStop = false;
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
            case MoveDirection.FORWORD:
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
