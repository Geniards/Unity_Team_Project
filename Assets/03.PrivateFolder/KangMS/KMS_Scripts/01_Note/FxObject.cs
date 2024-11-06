using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxObject : MonoBehaviour, IPoolingObj
{
    [SerializeField] private E_VFX _fxType;
    [SerializeField] private E_Pool _poolType;
    public E_Pool MyPoolType => _poolType;

    [SerializeField] private Animation _anim;

    public void Play()
    {
        _anim.Play();
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}



public interface IUIFxObject
{
    public Canvas Canvas { get; }
    public void SetPos(Vector3 standardPos);
}