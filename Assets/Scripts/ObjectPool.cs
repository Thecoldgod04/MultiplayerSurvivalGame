using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [field: SerializeField]
    public List<Poolable> ObjectsToPool { get; private set; }

    [field: SerializeField]
    public List<List<GameObject>> ObjectPoolList { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            ObjectPoolList = new List<List<GameObject>>();
        }
        else
            Destroy(this.gameObject);

        foreach(Poolable objectToPool in ObjectsToPool)
        {
            List<GameObject> temp = new List<GameObject>();
            for(int i = 0; i < objectToPool.poolAmount; i++)
            {
                GameObject go = Instantiate(objectToPool.gameObject);
                go.SetActive(false);
                temp.Add(go);
            }
            ObjectPoolList.Add(temp);
        }
    }

    public GameObject GetPooledObject(GameObject gameObject)
    {
        foreach(List<GameObject> objectPool in ObjectPoolList)
        {
            if(objectPool[0].name.Contains(gameObject.name))
            {
                foreach(GameObject pooledObject in objectPool)
                {
                    if(!pooledObject.activeInHierarchy)
                    {
                        return pooledObject;
                    }
                }
            }
        }
        return null;
    }

    public void AddToPool(GameObject gameObject)
    {
        foreach (List<GameObject> objectPool in ObjectPoolList)
        {
            if (objectPool[0].name.Contains(gameObject.name))
            {
                objectPool.Add(gameObject);
                return;
            }
        }
/*
        List<GameObject> newObjectPool = new List<GameObject>();
        newObjectPool.Add(gameObject);
        ObjectPoolList.Add(newObjectPool);*/
    }

    public void AddNewToPool(GameObject gameObject)
    {
        if(IsInPool(gameObject))
        {
            AddToPool(gameObject);
        }
        else
        {
            List<GameObject> newPool = new List<GameObject>();
            newPool.Add(gameObject);
            ObjectPoolList.Add(newPool);
        }
    }
    public bool IsInPool(GameObject gameObject)
    {
        foreach (List<GameObject> objectPool in ObjectPoolList)
        {
            //Debug.LogError(gameObject == null);
            if (objectPool[0].name.Contains(gameObject.name))
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class Poolable
{
    public GameObject gameObject;

    public int poolAmount;
}
