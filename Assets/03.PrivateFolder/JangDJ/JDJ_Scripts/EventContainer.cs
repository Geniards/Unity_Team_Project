using System;
using System.ComponentModel;
using UnityEngine;

public class EventContainer : MonoBehaviour, IPoolingObj
{
    [SerializeField] private MonoBehaviour _targetObj;
    private Action _myAction;
    
    public E_Pool MyPoolType => E_Pool.EVENT;
    public E_Event MyEvent { get; private set; } 

    /// <summary>
    /// 초기화 작업, 기능등록과 사용하는 대상을 저장합니다.
    /// </summary>
    public void Initialize(E_Event eventType, Action action, MonoBehaviour user)
    {
        this._myAction = action;
        _targetObj = user;
        MyEvent = eventType;
    }
    
    /// <summary>
    /// 해당 오브젝트가 지닌 기능을 동작합니다.
    /// 만약 
    /// </summary>
    public void Run()
    {
        if (AutoDestroy())
            return;

        _myAction?.Invoke();
    }

    /// <summary>
    /// 대상 기능과 해당 오브젝트가 갖고있는 기능이 일치한지 판단합니다.
    /// </summary>
    public bool IsSameAction(Action action)
    {
        return _myAction == action;
    }

    /// <summary>
    /// true = 대상 이벤트의 참조가 없으므로 삭제됨
    /// </summary>
    public bool AutoDestroy()
    {
        if (_targetObj == null || _targetObj.isActiveAndEnabled == false)
        {
            EventManager.Instance.RemoveContainer( MyEvent , this);

            Return();
            return true;
        }

        return false;
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}
