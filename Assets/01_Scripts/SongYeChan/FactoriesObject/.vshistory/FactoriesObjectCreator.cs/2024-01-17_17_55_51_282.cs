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
    }
}
