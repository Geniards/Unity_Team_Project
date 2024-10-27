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
    /// ?�작�??�시??_endPoint�??�하???�아가?�록 ?�정.
    /// </summary>
    protected virtual IEnumerator MoveToLeft(double startDspTime, double endDspTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (endPoint - startPosition).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPoint);
        Debug.Log($"출발 ?�간 : {AudioSettings.dspTime} , ?�착?�정?�간 : {(totalDistance / speed) + AudioSettings.dspTime}");

        while (!_isHit)
        {
            double currentDspTime = AudioSettings.dspTime;
            // ?��? ?�간??비�??�여 �??�레???�정 거리만큼 ?�동
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);
            transform.position = startPosition + direction * coveredDistance;
            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                Debug.Log($"?�트가 목표 지?�에 ?�착?? ?�착 ?�간: {currentDspTime}");
                //Destroy(gameObject);
                yield break;
            }
            // ?�스?�용 로그 출력
            //Debug.Log($"?�트 ?�동 �?- ?�재 dspTime: {currentDspTime}, 목표 ?�간: {endDspTime}");
            yield return null;
        }
    }
    /// <summary>
    /// 공통???�격 ?�정???�???�수 처리
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
        Debug.Log($"Hit??결과 : {decision}, ?�수 : {scoreValue}");
    }
    /// <summary>
    /// 버튼 ?�력???�른 ?�정 처리
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision);
    /// <summary>
    // ?�펙??처리 (?�니메이???�는 ?�티??
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("?�펙???�작");
    }
}







