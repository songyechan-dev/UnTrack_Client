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

    Coroutine coroutine;

    private float curTime = 0f;
    // 시작
    void Start()
    {
        sensorTransform = transform.Find("Sensor");
    }
    // 업데이트
    void Update()
    {
        SensorDetect();
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
                    myState = MyState.MOVE;
                    if (hit.transform.localRotation.eulerAngles.y != transform.localRotation.eulerAngles.y)
                    {
                        if (coroutine != null)
                        {
                            coroutine = StartCoroutine(Turn(hit.transform));
                        }
                        else
                        {
                            return;
                        }
                        
                    }
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
        if (myState == MyState.MOVE)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// SensorDetect() 함수를 통해 구해진 방향으로 회전
    /// </summary>
    void RotationToDirection()
    {
       
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

    IEnumerator Turn(Transform targetTransform)
    {
        Debug.Log("호출됨");
        float duration = 1.5f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetTransform.eulerAngles.y, 0);
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetTransform.position + new Vector3(0, targetTransform.localScale.y / 2 + transform.localScale.y / 2, 0);


        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }



}
