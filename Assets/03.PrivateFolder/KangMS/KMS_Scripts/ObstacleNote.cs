using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleNote : Note, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.OBSTACLE_NOTE;

    public override float OnDamage()
    {
        Debug.Log($"��ֹ��� �浹! ������ : {damage} ����");
        return damage;
    }

    public override void OnHit(E_NoteDecision decision) { Debug.Log("�μ����� �ʽ��ϴ�."); }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}
