using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T: Component
{
    private Queue<T> pool;
    private T objPrefab;
    private Transform parent;

    public GameObjectPool(T _objPrefab, int _size, Transform _parent = null)
    {
        pool = new Queue<T>();
        this.objPrefab = _objPrefab;
        this.parent = _parent;

        for (int i = 0; i < _size; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        T newObj = Object.Instantiate(objPrefab, parent);
        newObj.gameObject.SetActive(false);
        pool.Enqueue(newObj);
    }

    public T Get(Vector3 _spawnPos)
    {
        if (pool.Count == 0)
        {
            CreateObject();
        }
        
        T _obj = pool.Dequeue();
        _obj.transform.position = _spawnPos;
        _obj.gameObject.SetActive(true);
        return _obj;
    }

    public void Return(T _obj)
    {
        _obj.gameObject.SetActive(false);
        pool.Enqueue(_obj);
    }
}
