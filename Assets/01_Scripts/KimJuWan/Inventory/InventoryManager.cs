using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, List<GameObject>> materials = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<GameObject>> craft = new Dictionary<string, List<GameObject>> ();

    public GameObject railPrefab;
    
    public PlayerController playerController;
    void Start()
    {
        
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
        
    
}
