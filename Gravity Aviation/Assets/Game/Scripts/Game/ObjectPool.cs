using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<int> sizes;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            try
            {
                for (int j = 0; j < sizes[i]; j++)
                {
                    GameObject obj = Instantiate(prefabs[i]);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }
            catch (ArgumentOutOfRangeException)
            {                
                Debug.Log("Out");
            }

            poolDictionary.Add(prefabs[i], objectPool);
        }
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public GameObject GetRandomPrefab()
    {
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Count);
        return prefabs[randomIndex];
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public GameObject GetFromPool(GameObject prefab) 
    {
        if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0)
        {
            GameObject obj = poolDictionary[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            return obj;
        }
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void ReturnToPool(GameObject prefab, GameObject obj) 
    {
        obj.SetActive(false);
        
        if (poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("ObjectPool: Возвращаемый объект не соответствует ни одному из пулов.");
            Destroy(obj);
        }
    }
}
