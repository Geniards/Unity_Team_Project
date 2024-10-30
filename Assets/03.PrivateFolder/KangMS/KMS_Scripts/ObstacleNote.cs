using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleNote : Note, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.OBSTACLE_NOTE;

    public override float GetDamage()
    {
        Debug.Log($"장애물과 충돌! 데미지 : {damage} 전달");
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button) { Debug.Log("부서지지 않습니다."); }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    public override void ReturnToPool()
    {
        NoteMediator.Instance.Unregister(this);
        Return();
    }
}
