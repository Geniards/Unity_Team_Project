using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class DBScoreNote : Note, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.DBSCORE_NOTE;

    public override float OnDamage()
    {
        return damage;
    }

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
}
