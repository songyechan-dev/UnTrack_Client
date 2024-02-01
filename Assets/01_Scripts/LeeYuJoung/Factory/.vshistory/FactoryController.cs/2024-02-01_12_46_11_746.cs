using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using TMPro;
using LeeYuJoung;
using Photon.Pun;

public class FactoryController : MonoBehaviourPun
{
    public FactoryManager factoryManager;
    public Transform targetPosition;

    public GameObject descriptionText;
    public GameObject smokeEffect;

    public float currentTime = 0;
    public float generateTime;

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Start()
    {

        factoryManager = GetComponent<FactoryManager>();
        descriptionText = transform.GetChild(1).gameObject;
        smokeEffect = transform.GetChild(2).gameObject;
    }



    private void Update()
    {
        DescriptionActive();
    }

    // 제작소 제작 중 일 시 효과
    public void MachineOperation()
    {
        if(factoryManager != null)
        {
            StartCoroutine(GenerateEffect());
        }
        else
        {

        }
    }

    IEnumerator GenerateEffect()
    {
        int loopNum = 0;
        smokeEffect.SetActive(true);

        while (true)
        {
            if (currentTime >= factoryManager.generateTime)
            {
                smokeEffect.SetActive(false);
                currentTime = 0;
                break;
            }

            currentTime += Time.deltaTime + 0.5f;
            transform.DOScale(new Vector3(1.0f, 0.8f, 1.0f), 1.0f);
            yield return new WaitForSeconds(0.25f);

            transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
            yield return new WaitForSeconds(0.25f);

            // 무한 루프 방지 예외처리
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    public void DescriptionActive()
    {
        if (GetDistance(transform.position, targetPosition.position) <= 2.5f)
        {
            OpenDescription();
        }
        else
        {
            CloseDescription();
        }
    }

    public float GetDistance(Vector3 a, Vector3 b)
    {
        Vector3 dir = a- b;
        float dist = Vector3.Magnitude(dir);

        return dist;
    }

    public void OpenDescription()
    {
        if (factoryManager != null)
        {
            if (factoryManager.currentItemNum >= factoryManager.itemMaxVolume)
                descriptionText.GetComponent<TextMeshPro>().color = Color.red;
            descriptionText.GetComponent<TextMeshPro>().text = $"{factoryManager.currentItemNum}/{factoryManager.itemMaxVolume}";

        }
        else
        {
            descriptionText.GetComponent<TextMeshPro>().text = $"{StateManager.Instance().storages["WOOD"] + StateManager.Instance().storages["STEEL"]}/{StateManager.Instance().storageMaxVolume}";
        }

        descriptionText.SetActive(true);
        descriptionText.transform.DOScale(new Vector3(1.0f,1.0f,1.0f), 0.25f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
    }

    public void CloseDescription()
    {
        descriptionText.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).OnComplete(() => descriptionText.SetActive(false));
    }
}
