using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + new Vector3(0,0,100f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
