using LeeYuJoung;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PlayerController playerController;
    private InventoryManager inventoryManager;
    public Transform pickSlot;
    public Transform sensor;
    public Transform droppedSlotPrefab;
    float castRange = 1f;

    public ItemManager itemManager;

    


    private void Awake()
    {
        if ((photonView != null && photonView.IsMine))
        {
            sensor = transform.GetChild(0).gameObject.GetComponent<Transform>();
            playerController = GetComponent<PlayerController>();
            inventoryManager = GetComponentInChildren<InventoryManager>();
        }
        else if (photonView == null)
        {
            return;
        }
    }

    

    //플레이어의 정면에 장애물이 있는 경우 파괴
    public void CollectIngredient()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("PickSlot"));
        RaycastHit hit;
        Ray ray = new Ray(sensor.position, -sensor.up);

        if (Physics.Raycast(ray, out hit, castRange, layerMask))
        {
            Debug.Log("태그명 !!! ::: " + hit.transform.tag);
            int childCount = 0;
            switch (hit.transform.tag)
            {
                case "Obstacle":
                    if (hit.transform.GetComponent<PhotonView>() != null)
                    {
                        hit.transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer); // 소유자 변경
                    }
                    hit.transform.GetComponent<ObstacleManager>().ObstacleWorking(inventoryManager.itemType.ToString(), playerController);
                    if (hit.transform.GetComponent<ObstacleManager>().isWorking)
                        playerController.playerState = PlayerController.PLAYERSTATE.EQUIPMENTACTION;
                    else
                        playerController.playerState = PlayerController.PLAYERSTATE.PICK;
                    
                    break;
                case "Item":
                    if (hit.transform.GetComponent<PhotonView>() != null)
                    {
                        hit.transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer); // 소유자 변경
                    }
                    childCount = hit.transform.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        hit.transform.GetChild(i).transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer); // 소유자 변경
                    }
                    childCount = 0;
                    Debug.Log("호출됨");
                    if (!playerController.isPick)
                    {
                        playerController.isPick = true;
                        castRange = 2f;
                        inventoryManager.SaveInventory(hit.transform.gameObject);
                        hit.transform.SetParent(pickSlot.transform);
                        pickSlot.GetChild(0).transform.position = pickSlot.position;
                        playerController.playerState = PlayerController.PLAYERSTATE.PICKUP;
                    }
                    else
                    {
                        if (inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.WOOD) ||
                            inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.STEEL) ||
                            inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.DROPPEDTRACK))
                        {
                            if (inventoryManager.SaveInventory(hit.transform.gameObject))
                            {
                                hit.transform.SetParent(pickSlot.transform);
                                pickSlot.GetChild(inventoryManager.itemNum - 1).transform.position =
                                    pickSlot.position + (Vector3.up * (inventoryManager.itemNum - 1));
                            }
                        }
                        playerController.playerState = PlayerController.PLAYERSTATE.PICKUP;
                    }

                    break;
                case "Factory":
                    FactoryManager _fm = hit.transform.GetComponent<FactoryManager>();
                    if (_fm.isHeating && inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.BUCKET))
                    {
                        playerController.isPick = false;
                        castRange = 1f;

                        Destroy(pickSlot.GetChild(0).gameObject);
                        inventoryManager.OutInventory();
                        _fm.FireSuppression();
                    }

                    if (_fm.ItemUse() && !playerController.isPick)
                    {
                        playerController.isPick = true;
                        castRange = 2f;

                        GameObject _object = (_fm.ItemGenerate());
                        inventoryManager.SaveInventory(_object.transform.gameObject);
                        _object.transform.SetParent(pickSlot.transform);
                        _object.transform.position = pickSlot.position;
                        _object.transform.rotation = pickSlot.rotation;
                        _object.name = _fm.generateItem;
                    }

                    break;
                case "Storage":
                    if ((playerController.isPick && inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.WOOD) && photonView.IsMine) || (playerController.isPick && inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.STEEL) && photonView.IsMine))
                    {
                        Debug.Log("뭔가를 들고있다 " + inventoryManager.itemType.ToString());
                        playerController.playerState = PlayerController.PLAYERSTATE.DROP;
                        if (StateManager.Instance().IngredientAdd(inventoryManager.itemType.ToString(), inventoryManager.itemNum))
                        {
                            Debug.Log("뭔가를 들고있다2" + inventoryManager.itemType.ToString());
                            playerController.isPick = false;
                            castRange = 1f;

                            for (int i = 0; i < pickSlot.transform.childCount; i++)
                            {
                                PhotonNetwork.Destroy(pickSlot.transform.GetChild(i).gameObject);
                                inventoryManager.OutInventory();
                            }
                        }
                    }

                    break;
                case "DroppedTrack":

                    break;
                case "DroppedSlot":
                    if (hit.transform.GetComponent<PhotonView>() != null)
                    {
                        hit.transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer); // 소유자 변경
                    }
                    childCount = hit.transform.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        if (hit.transform.GetChild(i).transform.GetComponent<PhotonView>().IsOwnerActive)
                            hit.transform.GetChild(i).transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer); // 소유자 변경
                    }
                    childCount = 0;
                    InventoryManager droppedSlot = hit.transform.GetComponent<InventoryManager>();
                    Debug.Log("카운트 ::::" + droppedSlot.transform.childCount);
                    Debug.Log("카운트2 ::::" + droppedSlot.itemNum);

                    if (playerController.currentTime >= playerController.spaceTime && playerController.isPick)
                    {
                        //바닥에 놓은 아이템들 위에 쌓기
                        if (inventoryManager.itemType.Equals(droppedSlot.itemType))
                        {
                            
                            playerController.isPick = false;
                            castRange = 1f;
                            int n = pickSlot.transform.childCount;
                            for (int i = 0; i < n; i++)
                            {
                                droppedSlot.GetComponent<PhotonSyncManager>().DroppedSlotIn(droppedSlot.transform, pickSlot.transform.GetChild(0).gameObject);
                                //droppedSlot.DroppedSlotIn(pickSlot.transform.GetChild(0).gameObject);
                                pickSlot.transform.GetChild(0).GetComponent<PhotonSyncManager>().SetParent(droppedSlot.transform,false);
                                droppedSlot.transform.GetChild(droppedSlot.itemNum - 1).GetComponent<PhotonSyncManager>().SetPosition(droppedSlot.transform.position + (Vector3.up * (droppedSlot.itemNum - 1)));
                            }
                            inventoryManager.OutInventory();
                        }
                        playerController.playerState = PlayerController.PLAYERSTATE.DROP; ;
                    }
                    else
                    {
                        if (playerController.isPick)
                        {
                            if (inventoryManager.itemType.Equals(droppedSlot.itemType))
                            {
                                if (inventoryManager.itemNum < 4)
                                {
                                    inventoryManager.SaveInventory(hit.transform.GetChild(droppedSlot.itemNum - 1).gameObject);
                                    hit.transform.GetChild(droppedSlot.itemNum - 1).GetComponent<PhotonSyncManager>().SetParent(pickSlot,true);

                                    pickSlot.GetChild(inventoryManager.itemNum - 1).transform.GetComponent<PhotonSyncManager>().SetPosition(pickSlot.position + (Vector3.up * (inventoryManager.itemNum - 1)));
                                    droppedSlot.GetComponent<PhotonSyncManager>().DroppedSlotOut(droppedSlot.GetComponent<PhotonView>().ViewID);
                                }
                            }
                        }
                        else
                        {
                            
                            playerController.isPick = true;
                            castRange = 2.0f;

                            inventoryManager.SaveInventory(hit.transform.GetChild(droppedSlot.itemNum - 1).gameObject);
                            playerController.playerState = PlayerController.PLAYERSTATE.PICKUP;
                            //
                            hit.transform.GetChild(droppedSlot.itemNum - 1).GetComponent<PhotonSyncManager>().SetParent(pickSlot,true);
                            pickSlot.GetChild(0).GetComponent<PhotonSyncManager>().SetPosition(pickSlot.position);
                            //hit.transform.GetChild(droppedSlot.itemNum - 1).SetParent(pickSlot);
                            //pickSlot.GetChild(0).transform.position = pickSlot.position;
                            droppedSlot.GetComponent<PhotonSyncManager>().DroppedSlotOut(droppedSlot.GetComponent<PhotonView>().ViewID);
                        }
                        Debug.Log("ItemNum ::::" + droppedSlot.itemNum);
                        if (droppedSlot.itemNum <= 1)
                        {
                            Debug.Log("삭제되어야됩니다.");
                            DestroyOnNetwork(droppedSlot.transform.GetComponent<PhotonSyncManager>());
                        }

                    }
                    break;
                case "Plane":
                    Debug.Log("바닥입니다....");

                    if (playerController.isPick)
                    {
                        playerController.playerState = PlayerController.PLAYERSTATE.DROP;
                        if (inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.WOOD) || inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.STEEL))
                        {
                            playerController.isPick = false;
                            castRange = 1.0f;
                            GameObject _droppedSlot = PhotonNetwork.Instantiate("DroppedSlot", pickSlot.transform.position, Quaternion.identity);
                            _droppedSlot.name = "DroppedSlot";

                            int num = pickSlot.transform.childCount;
                            for (int i = 0; i < num; i++)
                            {
                                //_droppedSlot.GetComponent<InventoryManager>().DroppedSlotIn(pickSlot.transform.GetChild(0).gameObject);

                                //pickSlot.transform.GetChild(0).SetParent(_droppedSlot.transform);
                                
                                pickSlot.transform.GetChild(0).GetComponent<PhotonSyncManager>().DroppedSlotIn(_droppedSlot.transform, pickSlot.transform.GetChild(0).gameObject);
                                pickSlot.transform.GetChild(0).GetComponent<PhotonSyncManager>().SetParent(_droppedSlot.transform,false);




                                ObjectRotationCheck(_droppedSlot.transform.GetChild(i).gameObject);
                            }
                            inventoryManager.OutInventory();
                        }
                        else if (inventoryManager.itemType.Equals(ItemManager.ITEMTYPE.DROPPEDTRACK))
                        {
                            Debug.Log("생성이되나?");

                            if (hit.transform.CompareTag("Plane"))
                            {
                                int num = pickSlot.transform.childCount;

                                if (num <= 1)
                                {
                                    playerController.isPick = false;
                                    castRange = 1.0f;
                                    Debug.Log("여기는통과");
                                    DestroyOnNetwork(pickSlot.transform.GetChild(0).GetComponent<PhotonSyncManager>());
                                    inventoryManager.DroppedSlotOut();
                                    GameObject.Find("TrackManager").GetComponent<TrackManager>().TrackCreate(ray);
                                }
                                else
                                {
                                    Debug.Log("여기는통과");
                                    //_droppedSlot.GetComponent<InventoryManager>().DroppedSlotIn(pickSlot.transform.GetChild(i).gameObject);
                                    DestroyOnNetwork(pickSlot.transform.GetChild(num - 1).GetComponent<PhotonSyncManager>());
                                    inventoryManager.DroppedSlotOut();
                                    GameObject.Find("TrackManager").GetComponent<TrackManager>().TrackCreate(ray);
                                }
                            }

                        }
                        else
                        {
                            Debug.Log("여기호출인가");
                            playerController.isPick = false;
                            castRange = 1.0f;

                            inventoryManager.DroppedSlotIn(pickSlot.transform.GetChild(0).gameObject);
                            inventoryManager.OutInventory(pickSlot.transform.GetChild(0).gameObject);
                            ObjectRotationCheck(pickSlot.transform.GetChild(0).gameObject);
                            pickSlot.transform.GetChild(0).SetParent(null);
                        }
                    }

                    break;
            }
        }
        else
        {

        }
    }


    void DestroyOnNetwork(PhotonSyncManager _photonSyncManager)
    {
        _photonSyncManager.DestroyObject();
    }
    //플레이어가 바라보는 방향에 따라 아이템의 각도 변경
    public void ObjectRotationCheck(GameObject _gameObject)
    {
        float _rotation = transform.rotation.eulerAngles.y;
        Debug.Log(_rotation);

        if (_rotation >= 0.0f && _rotation < 85.0f)
        {
            _gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (_rotation >= 85.0f && _rotation < 175.0f)
        {
            _gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        else if (_rotation >= 175.0f && _rotation < 265.0f)
        {
            _gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if (_rotation >= 265.0f && _rotation < 360.0f)
        {
            _gameObject.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        }
    }

    //바닥 UI 인식
    //public void ButtonClick()
    //{
    //    //TODO 김주완 0119 : UI Manager에서 UI 기능들 끌어다 쓰게 하기
    //    RaycastHit hit;
    //    if(Physics.Raycast(transform.position, Vector3.down, out hit, castRange))
    //    {
    //        Debug.Log("버튼입니다...");
    //            playerController.keyCode++;
    //            if (playerController.keyCode > 2)
    //            {
    //                playerController.keyCode = 0;
    //            }

    //    }

    //}
}