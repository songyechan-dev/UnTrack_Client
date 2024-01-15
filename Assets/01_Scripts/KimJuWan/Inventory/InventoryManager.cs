using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, List<GameObject>> materials = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<GameObject>> craft = new Dictionary<string, List<GameObject>> ();

    public Dictionary<string, int> storage = new Dictionary<string, int>();

    public GameObject railPrefab;
    
    public PlayerController playerController;
    void Start()
    {
        storage.Add("Wood", 2);
        storage.Add("Steel", 1);
    }

    // 업데이트
    void Update()
    {
        
    }

    public void PlayerInventoryWood()
    {
        materials.Add("Wood", new List<GameObject> { playerController.nearObject });      
    }

    public void PlayerInventorySteel()
    {
        materials.Add("Steel", new List<GameObject> { playerController.nearObject});
    }

    public void FactoryInventoryRail() 
    {
        if (materials["Wood"].Count >=2 && materials["Steel"].Count>=3)
        {
            
            craft.Add("Rail", new List<GameObject> { railPrefab });
            
        }

        
    }
        
    // 저장소에 재료 저장
    public void SaveInventory(string _ingredient, int _amount)
    {
        storage[_ingredient] += _amount;
    }

    // 저장소에 있는 재료 사용
    public void UseInventory(string _ingredient, int _amount)
    {
        storage[_ingredient] -= _amount;
    }
}
