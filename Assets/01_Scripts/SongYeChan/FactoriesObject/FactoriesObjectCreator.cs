using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoriesObjectCreator : MonoBehaviour
{
    public GameObject factoriesObjectPrefab;
    void Create(Vector3 startedPosition)
    {
        GameObject _factoriesObject = Instantiate(factoriesObjectPrefab);
        _factoriesObject.transform.position = startedPosition;
        _factoriesObject.AddComponent<FactoriesObjectManager>();

        //gameManager 혹은 stateManager에서 init 에 해당하는 값 들고와서 _factoriesObject Init
        //_factoriesObject.GetComponent<FactoriesObjectManager>().Init();
    }
}
