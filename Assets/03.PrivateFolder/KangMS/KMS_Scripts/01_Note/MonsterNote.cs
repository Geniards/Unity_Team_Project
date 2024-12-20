using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNote : Note, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.MONSTER_NOTE;

    public override float GetDamage()
    {
        //Debug.Log($"몬스터와 충돌! 데미지 : {damage} 전달");
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {
        if (!_isHit)
        {
            _isHit = true;
            ShowEffect();

            gameObject.SetActive(false);
            ReturnToPool();
        }
    }

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
