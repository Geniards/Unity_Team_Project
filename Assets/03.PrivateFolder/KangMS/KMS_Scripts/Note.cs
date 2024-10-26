using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    public float speed = 5f;
    public float scoreValue = 100;
    public Vector3 endPoint;

    protected bool _isHit = false;

    public virtual void Initialize(Vector3 endPoint, float speed, float scoreValue)
    {
        this.endPoint = endPoint;
        this.speed = speed;
        this.scoreValue = scoreValue;

        Debug.Log($"중심 :{transform.position}");

        StartCoroutine(MoveToLeft());
    }

    /// <summary>
    /// 시작과 동시에 _endPoint를 향하여 날아가도록 설정.
    /// </summary>
    protected virtual IEnumerator MoveToLeft()
    {
        float moveNodeDistance = Vector3.Distance(transform.position, endPoint);
        float startTime = Time.time;

        while(!_isHit && transform.position != endPoint)
        {
            //float distNode = (Time.time - startTime) * speed;
            //float ratioDist = distNode / moveNodeDistance;
            //transform.position = Vector3.Lerp(transform.position, endPoint.position, ratioDist);
            
            // 매 프레임마다 일정한 속도로 좌측으로 이동
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPoint, step);

            if (Vector3.Distance(transform.position, endPoint) < 0.001f)
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
