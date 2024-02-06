using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MapAnimator : MonoBehaviour
{
    public GameObject[] clouds = new GameObject[4];
    public GameObject ground1;
    public GameObject ground2;
    public Transform camTransform;

    public Vector3[] cloudPos = new Vector3[4]
    {new Vector3(10.0f,11.0f, 35.0f), new Vector3(5.0f, 8.0f, 45.0f), new Vector3(0.0f, 11.0f, 40.0f), new Vector3(-5.0f, 8.0f, 40.0f) };

    public float ground1_posZ = 10.0f;
    public float ground2_posZ = -10.0f;

    public float shakeTime = 1.5f;
    public float shakeSpeed = 2.0f;
    public float shakeAmount = 2.5f;

    void Start()
    {
        ground1 = transform.GetChild(0).gameObject;
        ground2 = transform.GetChild(1).gameObject;
        camTransform = GameObject.Find("Main Camera").GetComponent<Transform>();

        BeginMapGenerate();
        StartCoroutine(CloudMove());
        StartCoroutine(CloudMove());
        StartCoroutine(CloudMove());
    }

    void Update()
    {

    }

    public void BeginMapGenerate()
    {
        ground1.transform.DOMoveX(ground1_posZ, 3.5f);
        ground2.transform.DOMoveX(ground2_posZ, 3.5f).OnComplete(CameraShakeCall);

        //ground1.transform.DOMoveZ(ground1_posZ, 2.0f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
        //ground2.transform.DOMoveZ(ground2_posZ, 2.0f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
    }

    IEnumerator CloudMove()
    {
        int _idx = Random.Range(0, clouds.Length);
        float _duration = Random.Range(25, 35);

        if (!clouds[_idx].activeSelf)
        {
            clouds[_idx].SetActive(true);
            clouds[_idx].transform.DOMoveZ(-40, _duration).OnComplete(() => clouds[_idx].transform.position = cloudPos[_idx]);

            yield return new WaitForSeconds(_duration);

            clouds[_idx].SetActive(false);
            StartCoroutine(CloudMove());
        }
        else
        {
            StartCoroutine(CloudMove());
        }
    }

    public void CameraShakeCall()
    {
        StartCoroutine(CameraShake());
    }

    IEnumerator CameraShake()
    {
        Vector3 originPosition = camTransform.localPosition;
        float currentTime = 0.0f;

        while(currentTime < shakeTime)
        {
            Vector3 randomPos = originPosition + Random.insideUnitSphere * shakeAmount;
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, randomPos, Time.deltaTime * shakeSpeed);

            yield return null;

            currentTime += Time.deltaTime;
        }

        camTransform.localPosition = originPosition;
    }
}
