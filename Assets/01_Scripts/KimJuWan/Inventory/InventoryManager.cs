using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEMTYPE
{
    WOOD = 1,
    STEEL,

    TRACK
}

public class InventoryManager : MonoBehaviour
{
    
    public Dictionary<ITEMTYPE, GameObject> playerIngredients = new Dictionary<ITEMTYPE, GameObject>();
    
    public Dictionary<string, int> playerStorage = new Dictionary<string, int>();
    //public GameObject railPrefab;
    public PlayerController playerController;
    


    void Start()
    {
        int woodItemCnt = 0;
        int steelItemCnt = 0;
        playerStorage.Add("Woods", woodItemCnt);
        playerStorage.Add("Steels", steelItemCnt);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        
        Debug.Log($"{playerIngredients.Count}");

        Debug.Log(playerStorage["Woods"]);
        Debug.Log(playerStorage["Steels"]);
        
        

    }

    //Player가 집은 오브젝트가 재료 아이템인 경우 저장소에 저장
    public void SavePlayerInventory()
    {
        
        if (playerController.nearObject != null || playerController.nearObject != null)
        {
            
                string itemName = playerController.nearObject.name;

                if (itemName == "Wood")
                {
                    ITEMTYPE itemType = ITEMTYPE.WOOD;

                    // 플레이어가 나무를 집은 경우
                    if (playerIngredients.ContainsKey(itemType))
                    {
                        playerIngredients[itemType] = playerController.nearObject;
                    }
                    else
                    {
                        playerIngredients.Add(itemType, playerController.nearObject);
                    }

                    playerStorage["Woods"]++;

                }
                else if (itemName == "Steel")
                {
                    ITEMTYPE itemType = ITEMTYPE.STEEL;

                    // 플레이어가 철을 집은 경우
                    if (playerIngredients.ContainsKey(itemType))
                    {
                        playerIngredients[itemType] = playerController.nearObject;
                    }
                    else
                    {
                        playerIngredients.Add(itemType, playerController.nearObject);
                    }

                    playerStorage["Steels"]++;
                }
 
        }
    }

    
    
}

    






