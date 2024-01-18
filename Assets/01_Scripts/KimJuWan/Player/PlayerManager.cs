using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    PlayerController playerController;
    ItemManager itemManager;
    


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        itemManager = GameObject.FindGameObjectWithTag("Item").GetComponent<ItemManager>();
    }

    // 업데이트
    void FixedUpdate()
    {
        

        
    }

    //플레이어의 정면에 장애물이 있는 경우 파괴
    public void CollectIngredient()
    {
        
        RaycastHit hit;
        int layerMask = (1 << LayerMask.NameToLayer("IgnoreRaycast"));
        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3f, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            switch (hitObject.tag)
            {
                case "Obstacle":
                    hitObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case "Item":
                    if (!playerController.isPick)
                    {
                        playerController.isPick = true;
                        
                       
                    }
                    if(playerController.isPick)
                    {
                        playerController.nearObject = hitObject;
                        playerController.items.Push(hitObject);
                        hitObject.transform.position = hitObject.transform.position;
                        hitObject.layer = 2;
                        hitObject.GetComponent<BoxCollider>().enabled = false;
                           
                            
                    }
                    break;
                case "Factory":
                    break;
                case "DroppedTrack":
                    break;
                default:
                    
                    break;
            }
        }
    }

    
}
