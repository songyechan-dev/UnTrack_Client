using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    private FactoryManager factoryManager;
    private FactoryManager storageFactoryManager;
    public float currentTime = 0;
    public bool isWorking = false;

    private void Start()
    {
        factoryManager = GetComponent<FactoryManager>();
        storageFactoryManager = GameObject.Find("Storage").GetComponent<FactoryManager>();
    }

    // ������ ���� �ð����� �ҳ��� �̺�Ʈ
    public void EngineOverheating()
    {

    }

    // Storage�� ��ᰡ ������� Ȯ�� 
    // �� �÷��̾ ����ҿ� ��� ���� �� & ������ ������ ������ �� Ȯ��
    public void IngredientCheck(string _ingredient1, string _ingredient2, int _amount1, int _amount2)
    {
        if (_ingredient1.Equals(_ingredient2))
        {
            if (storageFactoryManager.storages[_ingredient1] >= _amount1 + _amount2)
            {
                Debug.Log(":::: ���� ���� ::::");
                storageFactoryManager.IngredientUse(_ingredient1, _amount1 + _amount2);
            }
            else
            {
                Debug.Log(":::: ��ᰡ �����Ͽ� �������� ������ �� �����ϴ� ::::");
            }
        }
        else
        {
            if (storageFactoryManager.storages[_ingredient1] >= _amount1 && storageFactoryManager.storages[_ingredient2] >= _amount2)
            {
                Debug.Log(":::: ���� ���� ::::");
                storageFactoryManager.IngredientUse(_ingredient1, _amount1);
                storageFactoryManager.IngredientUse(_ingredient2, _amount2);
            }
            else
            {
                Debug.Log(":::: ��ᰡ �����Ͽ� �������� ������ �� �����ϴ� ::::");
            }
        }
    }

    // ������ ���� ����
    IEnumerator ItemProduction(float _generateTime, string _generateItem)
    {
        int loopNum = 0;

        while (true)
        {
            // ������ ���� ȿ�� ����

            yield return new WaitForEndOfFrame();
            Debug.Log("CurrentTime ::: " + currentTime);
            currentTime += Time.deltaTime;

            if (currentTime > _generateTime)
            {
                // ������ ���� �Ϸ�
                Debug.Log("Generate ::: " + _generateItem);
                currentTime = 0;
            }

            // ���� ���� ���� ����ó��
            if (loopNum++ > 10000)
                throw new Exception("Infinite Loop");
        }
    }

    // ������ ����
    public void ItemGenerate(string _generateItem)
    {
        GameObject _item = AssetDatabase.LoadAssetAtPath($"Assets/02_Prefabs/SongYeChan/{_generateItem}.prefab", typeof(GameObject)) as GameObject;
        GameObject _object = Instantiate(_item, transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.rotation);
        _object.name = _generateItem;
    }
}
