using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    public float speed = 5f;
    public float scoreValue = 100;
    public Transform endPoint;

    protected bool _isHit = false;

    public enum E_NoteDecision { None, Perfect, Great, Good, E_NOTEDECISION_MAX }

    public virtual void Initialize(Transform endPoint, float speed, float scoreValue)
    {
        this.endPoint = endPoint;
        this.speed = speed;
        this.scoreValue = scoreValue;

        StartCoroutine(MoveToLeft());
    }

    /// <summary>
    /// 시작과 동시에 _endPoint를 향하여 날아가도록 설정.
    /// </summary>
    protected virtual IEnumerator MoveToLeft()
    {
        float moveNodeDistance = Vector3.Distance(transform.position, endPoint.position);
        float startTime = Time.time;

        while(!_isHit && transform.position != endPoint.position)
        {
            float distNode = (Time.time - startTime) * speed;
            float ratioDist = distNode / moveNodeDistance;
            transform.position = Vector3.Lerp(transform.position, endPoint.position, ratioDist);
            
            if (ratioDist >= 1f)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 공통된 피격 판정에 대한 점수 처리
    /// </summary>
    protected virtual void CalculateScore(E_NoteDecision decision, bool isBoss = false)
    {
        if (isBoss)
        {
            if(decision == E_NoteDecision.Perfect)
                scoreValue *= (int)(decision) * 2;
            else
                scoreValue *= (int)((float)decision - (float)decision * 0.1f) * 2;
        }
        else
        {
            if (decision == E_NoteDecision.Perfect)
                scoreValue *= (int)(decision);
            else
                scoreValue *= (int)((float)decision - (float)decision * 0.1f);
        }
        Debug.Log($"Hit된 결과 : {decision}, 점수 : {scoreValue}");
    }

    /// <summary>
    /// 버튼 입력에 따른 판정 처리
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision, bool isBoss = false);

    /// <summary>
    // 이펙트 처리 (애니메이션 또는 파티클)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("이펙트 동작");
    }
}
