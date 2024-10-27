using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScoreNote : Note, IPoolingObj, IReflective
{
    public E_Pool MyPoolType => E_Pool.MONSTER;
    public E_NoteDecision NoteDecision;

    /// <summary>
    /// Test�� Ȯ���ϱ� ���� ���� ���� ���� �ʿ�!
    /// </summary>
    private void Awake()
    {
        // test��
        Initialize(endPoint, speed, scoreValue);
    }
    /// <summary>
    /// Test�� Ȯ���ϱ� ���� ���� ���� ���� �ʿ�!
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnHit(NoteDecision);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            NoteDecision = E_NoteDecision.Perfect;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            NoteDecision = E_NoteDecision.Great;
    }

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
        
    }
}
