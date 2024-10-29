using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScoreNote : Note, IPoolingObj, IReflective
{
    public E_Pool MyPoolType => E_Pool.SCORE_NOTE;

    public override void OnHit(E_NoteDecision decision)
    {
        if (!_isHit)
        {
            _isHit = true;
            CalculateScore(decision);
            ShowEffect();
            gameObject.SetActive(false);
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    public void ReflectNote()
    {
        Debug.Log("반사노트(검기노트)에 대한 오브젝트 풀로 전환 후 해당 노트는 삭제시킨다.");
    }

    public override float OnDamage(){ return damage; }
}
