using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
public abstract class Note : MonoBehaviour
{
    public float speed = 5f;
    public float scoreValue = 100;
    public Vector3 endPoint;
    public bool _isHit = false;
    public static bool isBoss = false;
    
    public virtual void Initialize(Vector3 endPoint, float speed, float scoreValue)
    {
        this.endPoint = endPoint;
        this.speed = speed;
        this.scoreValue = scoreValue;
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
        Debug.Log($"출발 시간 : {AudioSettings.dspTime} , 도착예정시간 : {(totalDistance / speed) + AudioSettings.dspTime}");

        while (!_isHit)
        {
            double currentDspTime = AudioSettings.dspTime;
            // 남은 시간에 비례하여 매 프레임 일정 거리만큼 이동
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);
            transform.position = startPosition + direction * coveredDistance;
            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                Debug.Log($"노트가 목표 지점에 도착함, 도착 시간: {currentDspTime}");
                Destroy(gameObject);
                yield break;
            }
            // 테스트용 로그 출력
            //Debug.Log($"노트 이동 중 - 현재 dspTime: {currentDspTime}, 목표 시간: {endDspTime}");
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
        Debug.Log($"Hit된 결과 : {decision}, 점수 : {scoreValue}");
    }
    /// <summary>
    /// 버튼 입력에 따른 판정 처리
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision);
    /// <summary>
    // 이펙트 처리 (애니메이션 또는 파티클)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("이펙트 동작");
    }
}







