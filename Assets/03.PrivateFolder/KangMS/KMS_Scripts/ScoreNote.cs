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
            Destroy(gameObject);
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    public void ReflectNote()
    {
        Debug.Log("�ݻ��Ʈ(�˱��Ʈ)�� ���� ������Ʈ Ǯ�� ��ȯ �� �ش� ��Ʈ�� ������Ų��.");
    }
}
