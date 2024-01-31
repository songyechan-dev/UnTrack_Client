using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FactoriesObjectManager : MonoBehaviourPun
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
    public string tagToBeDetected;
    [SerializeField]
    MoveDirection moveDirection;
    [SerializeField, Range(0f, 100f)]
    public float moveSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    public float fireTime;
    [SerializeField, Range(0f, 5f)]
    public float rotationTime = 10f;
    //[SerializeField, Range(0f, 500f)]
    //public float rotationPerFrame = 1.5f;
    //[SerializeField, Range(0f, 100f)]
    //public float rotationForwardSpeed = 0.1f;

    public bool isStop = false;
    public bool isChangedRotation = false;

    Transform sensorTransform;
    RaycastHit hit;


    Coroutine coroutine;

    private float curTime = 0f;
    // 시작
    void Awake()
    {
        sensorTransform = transform.Find("Sensor");
    }

    // 업데이트
    void Update()
    {
        if (gameObject.layer.ToString().Equals("FactoriesObject") && gameObject.tag != "FactoriesObject_First")
        {
            gameObject.tag = "FactoriesObject_First";
            Debug.Log("변경됨");
        }
        if (GameManager.Instance().gameState.Equals(GameManager.GameState.GameStart))
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
                    RaycastHit tempHit = new RaycastHit();
                    if (Physics.Raycast(new Ray(transform.position, -transform.up), out tempHit))
                    {
                        if (tempHit.transform != null && tempHit.transform.GetComponent<TrackInfo>() != null && tempHit.transform.GetComponent<TrackInfo>().isFinishedTrack)
                        {
                            myState = MyState.STOP;
                            GameManager.Instance().GameClear();
                            return;
                        }
                    }
                    if (hit.transform.GetComponent<TrackInfo>().isFinishedTrack)
                    {
                        myState = MyState.STOP;
                        GameManager.Instance().GameClear();
                        return;
                    }
                    else
                    {
                        myState = MyState.MOVE;
                        if (hit.transform.localRotation.eulerAngles.y != transform.localRotation.eulerAngles.y)
                        {
                            if (coroutine == null)
                            {
                                Turn(hit.transform);
                            }
                            else
                            {
                                return;
                            }
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
    /// <summary>
    /// FactoriesObject 이동 메서드
    /// </summary>
    void Move()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,1f))
        {
            if (hit.transform.tag != null && hit.transform.tag.Equals(this.transform.tag))
            {
                if (hit.transform.GetComponent<Rigidbody>() == null)
                {
                    hit.transform.AddComponent<Rigidbody>();
                    hit.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 5f);
                    //파괴 이벤트 발생
                }
            }
        }
        if (myState == MyState.MOVE)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// FactoriesObject의 각도가 현재 센서가 감지한 각도랑 일치하지 않을시 회전 및 포지션 변경하는 코루틴 호출
    /// </summary>
    /// <param name="_targetTransform"></param>
    public void Turn(Transform _targetTransform)
    {
        coroutine = StartCoroutine(TurnCoroutine(_targetTransform));
    }


    /// <summary>
    /// FactoriesObject의 각도가 현재 센서가 감지한 각도랑 일치하지 않을시 회전 및 포지션 변경
    /// </summary>
    /// <param name="_targetTransform">센서가 감지한 트랙의 Transform</param>
    /// <returns>yield return null</returns>
    IEnumerator TurnCoroutine(Transform _targetTransform)
    {
        Debug.Log("호출됨");
        float duration = rotationTime;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, _targetTransform.eulerAngles.y, 0);
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z);
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
        coroutine = null;
    }


    ///
    //TODO : gameManager Round 및 미터 가져와서 비교 2024.01.17 송예찬
    float increasingSpeed = 0.33f;
    float firstMoveSpeed = 0.15f;
    /// <summary>
    /// FactoriesObject Init
    /// </summary>
    /// <param name="round">현재난이도</param>
    /// <param name="meter">현재 미터</param>
    public void Init()
    {
        int step = 0;
        float meter = GameManager.Instance().GetMeter();
        int round = GameManager.Instance().GetRound();
        if (meter < 0.3f)
        {
            if (round == 1)
            {
                moveSpeed = firstMoveSpeed;
                return;
            }
            moveSpeed = round * increasingSpeed;
        }
        else
        {
            step = (int)(meter / 0.3f);
            moveSpeed = step * increasingSpeed * round;
        }
    }

 

}
