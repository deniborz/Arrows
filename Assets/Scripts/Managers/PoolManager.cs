using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PoolItem
{
    public GameObject PoolObject = null;
    public int Amount = 5;
}

public class PoolManager : MonoBehaviour
{
    public List<PoolItem> PoolItems = new List<PoolItem>();
    private List<PoolObject>[] _pool;

    void Awake()
    {
        InstatiatePool();
    }

    void InstatiatePool()
    {
        _pool = new List<PoolObject>[PoolItems.Count];
        for (int item = 0; item < PoolItems.Count; ++item)
        {
            PoolItem pi = PoolItems[item];
            List<PoolObject> list = new List<PoolObject>();
            for (int count = 0; count < pi.Amount; ++count)
            {
                GameObject obj = Instantiate(pi.PoolObject, Vector3.zero, Quaternion.identity) as GameObject;
                obj.SetActive(false);
				obj.transform.SetParent(this.transform);

                list.Add(obj.GetComponent<PoolObject>());
            }
            _pool[item] = list;
        }
    }
    PoolObject AddExtraObjectToPool(int id)
    {
        GameObject obj = Instantiate(PoolItems[id].PoolObject, Vector3.zero, Quaternion.identity) as GameObject;
        obj.SetActive(false);
		obj.transform.SetParent(this.transform);
        _pool[id].Add(obj.GetComponent<PoolObject>());
        return obj.GetComponent<PoolObject>();
    }

    public PoolObject ActivateObject(int id)
    {
        if (id > PoolItems.Count || id < 0)
        {
            Debug.Log("Requesting illegal object with id " + id);
            return null;
        }
        for (int count = 0; count < _pool[id].Count; ++count)
        {
            PoolObject obj = _pool[id][count];
            if (obj == null)
            {
                Debug.Log("Object does not have a PoolObject script");
                return null;
            }
            if (!obj.IsActive)
            {
                obj.Reset();
                obj.Activate();
                return obj;
            }
        }
        return AddExtraObjectToPool(id);
    }
    public PoolObject ActivateObject(string name)
    {
        int id = FindIDByName(name);
        return ActivateObject(id);
    }

    public int FindIDByName(string name)
    {
        for (int count = 0; count < PoolItems.Count; ++count)
        {
            if (PoolItems[count].PoolObject.name.CompareTo(name) == 0)
                return count;
        }
        Debug.Log("Can't find " + name);
        return -1;
    }

    public List<PoolObject> GetAllActiveObjects(int id)
    {
        List<PoolObject> list = new List<PoolObject>();
        if (id > PoolItems.Count || id < 0)
        {
            Debug.Log("Requesting illegal object with id " + id);
            return null;
        }
        for (int count = 0; count < _pool[id].Count; ++count)
        {
            PoolObject obj = _pool[id][count];
            if (obj.IsActive)
                list.Add(obj);
        }
        return list;
    }
    public List<PoolObject> GetAllActiveObjects(string name)
    {
        int id = FindIDByName(name);
        return GetAllActiveObjects(id);
    }
}