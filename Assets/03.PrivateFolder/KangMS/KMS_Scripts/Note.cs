using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    [Header("노트 세부 조정")]
    public float speed = 5f;
    public float scoreValue = 100;
    public float damage = 0;
    public Vector3 endPoint;

    [Header("충돌 가능한 노트 유/무")]
    public bool _isHit = false;
    [Header("보스 출연 유/무")]
    public static bool isBoss = false;   //false가 기본값

    // 이동상태 제어 변수.
    protected bool isMoving = true;
    protected float length;

    public virtual void Initialize(Vector3 endPoint, float speed, float scoreValue, float damage = 0, float length = 0)
    {
        gameObject.SetActive(true);

        this.endPoint = endPoint;
        this.speed = speed;
        this.scoreValue = scoreValue;
        this.damage = damage;
        this.length = length;

        double startDspTime = AudioSettings.dspTime;
        double travelDuration = Vector3.Distance(transform.position, endPoint) / speed;
        double endDspTime = startDspTime + travelDuration;

        StartCoroutine(MoveToLeft(startDspTime, endDspTime));
    }

    public virtual void Initialize(Transform bossTransform, float speed, float scoreValue, float damage = 0, float length = 0)
    {
        gameObject.SetActive(true);

        this.endPoint = bossTransform.position;
        this.speed = speed;
        this.scoreValue = scoreValue;
        this.damage = damage;
        this.length = length;

        double startDspTime = AudioSettings.dspTime;
        double travelDuration = Vector3.Distance(transform.position, endPoint) / speed;
        double endDspTime = startDspTime + travelDuration;

        StartCoroutine(MoveToLeft(startDspTime, endDspTime));
    }

    /// <summary>
    /// 시작과 동시에 _endPoint를 향하여 날아가도록 설정.
    /// </summary>
    protected virtual IEnumerator MoveToLeft(double startDspTime, double endDspTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (endPoint - startPosition).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPoint);

        while (!_isHit && isMoving && Vector3.Distance(transform.position, endPoint) > 0.001f)
        {
            double currentDspTime = AudioSettings.dspTime;

            // 남은 시간에 비례하여 매 프레임 일정 거리만큼 이동
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);

            transform.position = startPosition + direction * coveredDistance;


            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                Debug.Log($"노트가 목표 지점에 도착함, 도착 시간: {currentDspTime}");

                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 공통된 피격 판정에 대한 점수 처리
    /// </summary>
    protected virtual void CalculateScore(E_NoteDecision decision)
    {
        if (isBoss)
        {
            scoreValue *= (float)decision * 2;
        }
        else
        {
            scoreValue *= (float)decision;
        }
        //Debug.Log($"Hit된 결과 : {decision}, 점수 : {scoreValue}");
    }

    /// <summary>
    /// 버튼 입력에 따른 판정 처리
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision, E_Boutton button = E_Boutton.None);
    public abstract float OnDamage();

    /// <summary>
    // 이펙트 처리 (애니메이션 또는 파티클)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("이펙트 동작");
    }
}
