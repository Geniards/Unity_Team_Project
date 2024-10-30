using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour, IManager
{
    /*
     * 풀링되어지는 대상
     * 몬스터, 장애물, 이펙트, 백그라운드용 오브젝트??
     */

    private static ObjPoolManager _instance = null;

    public static ObjPoolManager Instance => _instance;

    private Dictionary<E_Pool, ObjectPool> _pools = new Dictionary<E_Pool, ObjectPool>();
    [SerializeField] private List<PrefabItem> _prefabs = null;

    public void Init()
    {
        _instance = this;
        InitializePools();
    }

    /// <summary>
    /// 내부의 각 풀들을 초기화, 생성합니다.
    /// </summary>
    public void InitializePools()
    {
        AutoRegistPools();
    }

    private void AutoRegistPools()
    {
        PrefabItem item = null;
        ObjectPool newPool = null;

        for (int i = 0; i < _prefabs.Count; i++)
        {
            item = _prefabs[i];

            if (E_Pool.NONE >= item.TargetPool || item.TargetPool >= E_Pool.E_POOL_Max 
                || item.TargetPrefab == null)
            { throw new System.Exception("프리팹 등록 오류 재확인 요망"); }

            newPool = new ObjectPool(DataManager.Instance.ObjpoolInitCreateCount, item.TargetPrefab);
            _pools.Add(item.TargetPool, newPool);
        }
    }

    /// <summary>
    /// 요청하는 오브젝트 풀을 대상으로 객체를 반환 받습니다.
    /// </summary>
    public GameObject GetObject(E_Pool poolType)
    {
        return GetObject<GameObject>(poolType);
    }

    public GameObject GetObject(E_Pool poolType, Transform parent)
    {
        GameObject obj = GetObject(poolType);
        obj.transform.SetParent(parent);

        return obj;
    }

    /// <summary>
    /// 요청하는 오브젝트 풀을 대상으로 객체의 컴포넌트를 반환받습니다.
    /// </summary>
    public T GetObject<T>(E_Pool poolType)
    {
        GameObject obj = _pools[poolType].GetObject();

        if (obj.TryGetComponent<T>(out T type))
            return type;

        throw new System.Exception("대상 풀링 오브젝트의 요청하는 컴포넌트가 없음");
    }

    /// <summary>
    /// 정해진 풀타입을 기반으로 자신에게 맞는 풀을 찾아 반환시킵니다.
    /// </summary>
    public void ReturnObj(E_Pool poolType,GameObject obj)
    {
        _pools[poolType].ReturnObj(obj);
    }

    
}