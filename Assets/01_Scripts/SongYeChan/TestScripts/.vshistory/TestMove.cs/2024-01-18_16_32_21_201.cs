using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private TrackManager trackManager;
    float x => Input.GetAxis("Horizontal");
    float y => Input.GetAxis("Vertical");
    float moveSpeed = 10f;
    private CharacterController characterController

    void Start()
    {
        trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
    }

    void Update()
    {
        transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * moveSpeed);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            trackManager.TrackCreate(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

    }
}
