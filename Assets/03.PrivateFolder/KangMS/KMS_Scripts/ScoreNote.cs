using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScoreNote : Note, IPoolingObj, IReflective
{
    [Header("검기 노트 프리팹")]
    [SerializeField] private GameObject swordWaveNotePrefab;
    [SerializeField] private Transform bossTransform;

    public E_Pool MyPoolType => E_Pool.SCORE_NOTE;

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {
        if (!_isHit)
        {
            _isHit = true;
            CalculateScore(decision);
            ShowEffect();

            if (isBoss)
            {
                ReflectNote();
            }

            ReturnToPool();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    public void ReflectNote()
    {
        Debug.Log("반사노트(검기노트)에 대한 오브젝트 풀로 전환 후 해당 노트는 삭제시킨다.");
        if (swordWaveNotePrefab != null)
        {
            GameObject swordWaveNote = Instantiate(swordWaveNotePrefab, transform.position, Quaternion.identity);
            SwordWaveNote swordWave = swordWaveNote.GetComponent<SwordWaveNote>();
            swordWave.InitializeSwordWave(bossTransform, speed, scoreValue, damage); // 보스 위치를 목표로 설정
        }
    }

    public override float GetDamage(){ return damage; }

    public override void ReturnToPool()
    {
        GameManager.Mediator.Unregister(this);
        Return();
    }
}
