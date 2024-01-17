using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    PlayerController playerController;
    
    


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        
    }

    // 업데이트
    void FixedUpdate()
    {
        CollectIngredient();

        
    }

    //플레이어의 정면에 장애물이 있는 경우 파괴
    public void CollectIngredient()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Obstacle"))
            {
                hitObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else if (hitObject.CompareTag("Item"))
            {
                //테스트
                playerController.nearObject = hitObject ;
                playerController.isPick = true;
                
            }
            else if (hitObject.CompareTag("Factory"))
            {

            }
            else if (hitObject.CompareTag("DroppedTrack"))
            {

            }
            
          
        }
    }

    
}
