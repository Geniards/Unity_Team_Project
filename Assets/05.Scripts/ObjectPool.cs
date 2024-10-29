using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private int _createCount;
    private List<GameObject> _pool = null;
    private GameObject _prefab = null;
    private Transform _myDirectory = null;

    public ObjectPool(int initCreateCount, GameObject prefabObject)
    {
        _prefab = prefabObject;
        _createCount = initCreateCount;
        CreateMyDirectory();

        _pool = new List<GameObject>(_createCount * 2);
        CreateObject(_createCount);
    }

    private void CreateMyDirectory()
    {
        _myDirectory = new GameObject().transform;
        _myDirectory.SetParent(ObjPoolManager.Instance.transform);
        _myDirectory.name = $"{_prefab.name} pool"; 
    }

    private void CreateObject(int createCount)
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject newObj = GameObject.Instantiate(_prefab);

            _pool.Add(newObj);
            newObj.transform.SetParent(_myDirectory.transform);
            newObj.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 풀로 부터 오브젝트를 반환받습니다.
    /// </summary>
    public GameObject GetObject()
    {
        GameObject obj = null;

        if(_pool.Count <= 0)
        {
            _createCount *= 2;
            CreateObject(_createCount);

            return GetObject();
        }

        obj = _pool[_pool.Count - 1];
        _pool.RemoveAt(_pool.Count - 1);

        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 해당 오브젝트를 대상 풀로 반환시킵니다.
    /// </summary>
    public void ReturnObj(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Add(obj);
    }
}
