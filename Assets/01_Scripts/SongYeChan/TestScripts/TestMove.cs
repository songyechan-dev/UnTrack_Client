using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    // Start is called before the first frame update
    float x => Input.GetAxis("Horizontal");
    float y => Input.GetAxis("Vertical");
    float moveSpeed = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * moveSpeed);
    }
}
