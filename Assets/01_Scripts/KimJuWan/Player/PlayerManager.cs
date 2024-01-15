using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        IDLE = 0,
        WALK,
        PICK,
        AX
    }

    // 시작
    void Start()
    {
        
    }

    // 업데이트
    void FixedUpdate()
    {
        CollectIngredient();

        
    }

    public void CollectIngredient()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Obstacle"))
            {
                Destroy(hitObject, 1.5f);
            }
           
            

           
        }
    }
}
