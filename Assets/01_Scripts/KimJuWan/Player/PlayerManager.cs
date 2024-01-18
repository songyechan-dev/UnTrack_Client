using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerManager : MonoBehaviour
{
    
    PlayerController playerController;
    private InventoryManager inventoryManager;
    public Transform pickSlot;
    public Transform droppedSlotPrefab;
    float castRange = 1f;
    
    public ItemManager itemManager;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        
    }

    // 업데이트
    void FixedUpdate()
    {
        Debug.Log(inventoryManager.items.Count);

        
    }

    //플레이어의 정면에 장애물이 있는 경우 파괴
    public void CollectIngredient()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("PickSlot"));
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, castRange, layerMask))
        {
            
            
            switch (hit.transform.tag)
            {
                case "Obstacle":
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case "Item":
                    if (!playerController.isPick)
                    {
                        playerController.isPick = true;
                        castRange = 2f;
                        inventoryManager.SaveInventory(hit.transform.gameObject);
                        hit.transform.SetParent(pickSlot.transform);

                    }
                    else
                    {
                        if(inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(ItemManager.ITEMTYPE.WOOD) ||
                            inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(ItemManager.ITEMTYPE.STEEL))
                        {
                            if (inventoryManager.items.Peek().GetComponent<ItemManager>().itemType
                            .Equals(hit.transform.GetComponent<ItemManager>().itemType))
                            {
                                if (inventoryManager.items.Count < 4)
                                {

                                    inventoryManager.SaveInventory(hit.transform.gameObject);
                                    hit.transform.position = pickSlot.position + new Vector3(0, 1f, 0);
                                    hit.transform.SetParent(pickSlot.transform);
                                }
                            }
                            else
                            {
                                Debug.Log("Can't Carry items!!");
                                
                            }
                        }
                        
                            
                    }  
                    break;
                case "Factory":
                    break;
                case "Storage":
                    if (inventoryManager.items.Count > 0)
                    {
                        playerController.isPick = false;
                        castRange = 1f;

                        for (int i = 0; i < pickSlot.transform.childCount; i++)
                        {
                            Destroy(pickSlot.transform.GetChild(i).gameObject);
                        }
                        inventoryManager.OutInventory();
                    }
                    break;
                case "DroppedTrack":
                    break;
                case "DroppedSlot":
                    InventoryManager dropSlot = hit.transform.GetComponent<InventoryManager>();
                    
                    if (playerController.currentTime >= playerController.spaceTime && playerController.isPick)
                    {
                        // 아이템 더미 위에 더 올리기 
                        Debug.Log("::: 더 올려 놓기 :::");
                        if (inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(dropSlot.items.Peek().GetComponent<ItemManager>().itemType))
                        {
                            playerController.isPick = false;
                            castRange = 1.0f;
                        int num = pickSlot.transform.childCount;

                        for (int i = 0; i < num; i++)
                        {
                            dropSlot.DroppedSlotIn(pickSlot.transform.GetChild(0).gameObject);
                            pickSlot.transform.GetChild(0).SetParent(dropSlot.transform);
                        }

                        inventoryManager.OutInventory();
                    }
                        else
                    {
                        Debug.Log(":::: 같은 아이템이 아니라 올릴 수 없습니다 ::::");
                    }

            }
                    else
                    {
                        // 아이템 더미 위에서 하나 가져오기
                        Debug.Log("::: 가져 오기 :::");
                        playerController.isPick = true;
                        if (playerController.isPick)
                        {
                            if (inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(dropSlot.items.Peek().GetComponent<ItemManager>().itemType))
                            {
                                if (inventoryManager.items.Count < 4)
                                {
                                    int num = pickSlot.transform.childCount;

                                    dropSlot.DroppedSlotout().transform.SetParent(pickSlot.transform);
                                    inventoryManager.SaveInventory(pickSlot.GetChild(num).gameObject);
                                }
                                else
                                {
                                    
                                }
                            }
                            else
                            {
                                //TODO 김주완 0118: 다른 종류의 아이템을 들려고 할 때 경고 문구 출력 -> UI추가
                            }
                        }
                        else
                        {
                            playerController.isPick = true;
                            castRange = 2f;

                            if (inventoryManager.items.Count < 4)
                            {
                                int num = pickSlot.transform.childCount;

                                dropSlot.DroppedSlotout().transform.SetParent(pickSlot.transform);
                                inventoryManager.SaveInventory(pickSlot.GetChild(num).gameObject);
                            }
                            else{
                                
                            }
                        }
                    }
                    if(dropSlot.items.Count<=0)
                        Destroy(dropSlot.gameObject);
                    break;
            }
        }
        else
        {
            Debug.Log("바닥입니다....");

            if (playerController.isPick)
            {
                if (inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(ItemManager.ITEMTYPE.WOOD) || inventoryManager.items.Peek().GetComponent<ItemManager>().itemType.Equals(ItemManager.ITEMTYPE.STEEL))
                {
                    playerController.isPick = false;
                    castRange = 1.0f;
                    GameObject _prefab = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/KimJuWan/DroppedSlot.prefab", typeof(GameObject)) as GameObject;
                    GameObject _droppedSlot = Instantiate(_prefab, pickSlot.transform.position, Quaternion.identity);
                    _droppedSlot.name = "DroppedSlot";

                    int num = pickSlot.transform.childCount;
                    for (int i = 0; i < num; i++)
                    {
                        _droppedSlot.GetComponent<InventoryManager>().DroppedSlotIn(pickSlot.transform.GetChild(0).gameObject);
                        pickSlot.transform.GetChild(0).SetParent(_droppedSlot.transform);
                    }

                    inventoryManager.OutInventory();
                }
                else
                {
                    playerController.isPick = false;
                    castRange = 1.0f;
                    pickSlot.transform.GetChild(0).SetParent(null);
                    inventoryManager.OutInventory();
                }
            }
        }
    }
}