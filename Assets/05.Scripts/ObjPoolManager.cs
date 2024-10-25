using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour
{
    /*
     * Ǯ���Ǿ����� ���
     * ����, ��ֹ�, ����Ʈ, ��׶���� ������Ʈ??
     */

    private static ObjPoolManager _instance = null;

    public static ObjPoolManager Instance => _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private const int INIT_POOL_COUNT = 5;

    private Dictionary<E_Pool, ObjectPool> _pools = new Dictionary<E_Pool, ObjectPool>();
    [SerializeField] private List<PrefabItem> _prefabs = null;

    private void Start()
    {
        Initalize();
    }

    /// <summary>
    /// ������ �� Ǯ���� �ʱ�ȭ, �����մϴ�.
    /// </summary>
    public void Initalize()
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
            { throw new System.Exception("������ ��� ���� ��Ȯ�� ���"); }

            newPool = new ObjectPool(INIT_POOL_COUNT, item.TargetPrefab);
        }
    }

    /// <summary>
    /// ��û�ϴ� ������Ʈ Ǯ�� ������� ��ü�� ��ȯ �޽��ϴ�.
    /// </summary>
    public GameObject GetObject(E_Pool poolType)
    {
        return GetObject<GameObject>(poolType);
    }

    /// <summary>
    /// ��û�ϴ� ������Ʈ Ǯ�� ������� ��ü�� ������Ʈ�� ��ȯ�޽��ϴ�.
    /// </summary>
    public T GetObject<T>(E_Pool poolType)
    {
        GameObject obj = _pools[poolType].GetObject();

        if (obj.TryGetComponent<T>(out T type))
            return type;

        throw new System.Exception("��� Ǯ�� ������Ʈ�� ��û�ϴ� ������Ʈ�� ����");
    }

    /// <summary>
    /// ������ ǮŸ���� ������� �ڽſ��� �´� Ǯ�� ã�� ��ȯ��ŵ�ϴ�.
    /// </summary>
    public void ReturnObj(E_Pool poolType,GameObject obj)
    {
        _pools[poolType].ReturnObj(obj);
    }
}