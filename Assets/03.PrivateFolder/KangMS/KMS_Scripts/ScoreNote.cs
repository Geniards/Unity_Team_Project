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

    //테스트 코드 삭제요망
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            Initialize(endPoint, speed, scoreValue);

        if (Input.GetKeyDown(KeyCode.S))
            OnHit(E_NoteDecision.Perfect);
    }

    public override void OnHit(E_NoteDecision decision)
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

            gameObject.SetActive(false);
            Return();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    public void ReflectNote()
    {
        Debug.Log("반사노트(검기노트)에 대한 오브젝트 풀로 전환 후 해당 노트는 삭제시킨다.");
        if (swordWaveNotePrefab != null && bossTransform != null)
        {
            GameObject swordWaveNote = Instantiate(swordWaveNotePrefab, transform.position, Quaternion.identity);
            SwordWaveNote swordWave = swordWaveNote.GetComponent<SwordWaveNote>();
            swordWave.InitializeSwordWave(bossTransform, speed, scoreValue, damage); // 보스 위치를 목표로 설정
        }
    }

    public override float OnDamage(){ return damage; }
}
