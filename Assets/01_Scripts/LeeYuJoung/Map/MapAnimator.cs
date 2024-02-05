using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapAnimator : MonoBehaviour
{
    public GameObject[] clouds = new GameObject[4];
    public GameObject ground1;
    public GameObject ground2;

    public float ground1_posZ = 10.0f;
    public float ground2_posZ = -10.0f;

    void Start()
    {
        ground1 = transform.GetChild(0).gameObject;
        ground2 = transform.GetChild(1).gameObject;

        BeginMapGenerate();
        CloudMove();
    }

    void Update()
    {
        
    }

    public void BeginMapGenerate()
    {
        ground1.transform.DOMoveZ(ground1_posZ, 2.5f);
        ground2.transform.DOMoveZ(ground2_posZ, 2.5f);

        //ground1.transform.DOMoveZ(ground1_posZ, 2.0f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
        //ground2.transform.DOMoveZ(ground2_posZ, 2.0f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
    }

    public void CloudMove()
    {
        float _duration = Random.Range(5, 10);
        clouds[0].transform.DOMoveZ(-40, _duration);
    }
}
