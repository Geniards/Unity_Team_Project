using UnityEngine;

public class EXPooledObject : MonoBehaviour, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.MONSTER_NOTE;

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    private void OnDisable()
    {
        Return();
    }
}
