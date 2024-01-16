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
            
            //01.16 수정사항-> Obstacle 이름에 따라 ObstacleManager에서 함수 실행
            if (hitObject.name =="Tree")
            {
                hitObject.GetComponent<ObstacleManager>().ObstacleWorking("Ax");
            }
            else if (hitObject.name == "Stone")
            {
                hitObject.GetComponent<ObstacleManager>().ObstacleWorking("Pick");
            }
            
            if(hitObject.name == "Wood" || hitObject.name == "Steel")
            {
                playerController.isPick = true;
                hitObject = playerController.grabedObject.gameObject;
            }

           
        }
    }
}
