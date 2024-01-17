using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    PlayerController playerController;
    InventoryManager inventoryManager;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    // 업데이트
    void FixedUpdate()
    {
        CollectIngredient();

        Store();
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
            
            //01.16 수정사항-> Obstacle 이름에 따라 ObstacleManager에서 함수 실행
            if (hitObject.name =="Tree")
            {
                hitObject.GetComponent<ObstacleManager>().ObstacleWorking("Ax");
            }
            else if (hitObject.name == "Stone")
            {
                hitObject.GetComponent<ObstacleManager>().ObstacleWorking("Pick");
            }
            
            if(hitObject.name == "Wood")
            {
                playerController.isPickWood = true;

                playerController.nearObject = hitObject;
                

                if (playerController.pickedWoods.Count >= 3)
                {
                    playerController.isPickWood = false;
                    playerController.nearObject = null;
                }
            }
            else if(hitObject.name == "Steel")
            {
                playerController.isPickSteel = true;

                playerController.nearObject = hitObject;
                
                if (playerController.pickedSteels.Count >= 3)
                {
                    playerController.isPickSteel = false;
                    
                }
            }
          
        }
    }

    public void Store()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward, out hit, 1f))
        {
            GameObject hitFactory = hit.collider.gameObject;
            if (hitFactory.CompareTag("Storage"))
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerController.pickedWoods.RemoveRange(0, 2);
                    playerController.pickedSteels.RemoveRange(0, 2);
                    inventoryManager.playerStorage["Woods"] = 0;
                    inventoryManager.playerStorage["Steels"] = 0;
                    playerController.Drop();
                }

            }
        }
    }
}
