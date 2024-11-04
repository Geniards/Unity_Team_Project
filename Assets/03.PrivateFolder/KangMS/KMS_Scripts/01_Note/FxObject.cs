using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxObject : MonoBehaviour, IPoolingObj
{
    [SerializeField] private E_VFX _fxType;
    [SerializeField] private E_Pool _poolType;
    public E_Pool MyPoolType => _poolType;

    public Animation Anim { get; private set; }

    private void Start()
    {
        Anim = GetComponentInChildren<Animation>();
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}
