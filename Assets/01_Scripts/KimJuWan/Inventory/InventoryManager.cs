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
    
    public Dictionary<ITEMTYPE, GameObject> ingredients = new Dictionary<ITEMTYPE, GameObject>();
    
    public Dictionary<string, int> storage = new Dictionary<string, int>();
    //public GameObject railPrefab;
    public PlayerController playerController;
    


    void Start()
    {
        int woodItemCnt = 0;
        int steelItemCnt = 0;
        storage.Add("Woods", woodItemCnt);
        storage.Add("Steels", steelItemCnt);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        
        Debug.Log($"{ingredients.Count}");

        Debug.Log(storage["Woods"]);
        Debug.Log(storage["Steels"]);
        
        

    }

    //Player�� ���� ������Ʈ�� ��� �������� ��� ����ҿ� ����
    public void SaveInventory()
    {
        
        if (playerController.grabedObject != null)
        {
            string itemName = playerController.grabedObject.name;

            if (itemName == "Wood")
            {
                ITEMTYPE itemType = ITEMTYPE.WOOD;

                // �÷��̾ ������ ���� ���
                if (ingredients.ContainsKey(itemType))
                {
                    ingredients[itemType] = playerController.grabedObject;
                }
                else
                {
                    ingredients.Add(itemType, playerController.grabedObject);
                }

                storage["Woods"]++;
                
            }
            else if (itemName == "Steel")
            {
                ITEMTYPE itemType = ITEMTYPE.STEEL;

                // �÷��̾ ö�� ���� ���
                if (ingredients.ContainsKey(itemType))
                {
                    ingredients[itemType] = playerController.grabedObject;
                }
                else
                {
                    ingredients.Add(itemType, playerController.grabedObject);
                }

                storage["Steels"]++;
            }
        }
    }

    //�κ��丮�� ����� ���� ����� ���� factory�� ����
    public void UseInventory(string _ingredient, int _amount)
    {
        storage[_ingredient] = _amount;
        
    }

}

    






