using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleNote : Note, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.MONSTER_NOTE;

    public override void OnHit(E_NoteDecision decision) { }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}
