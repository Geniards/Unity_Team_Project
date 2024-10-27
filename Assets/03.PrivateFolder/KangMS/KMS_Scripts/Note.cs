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
    /// ?œì‘ê³??™ì‹œ??_endPointë¥??¥í•˜??? ì•„ê°€?„ë¡ ?¤ì •.
    /// </summary>
    protected virtual IEnumerator MoveToLeft(double startDspTime, double endDspTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (endPoint - startPosition).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPoint);
        Debug.Log($"ì¶œë°œ ?œê°„ : {AudioSettings.dspTime} , ?„ì°©?ˆì •?œê°„ : {(totalDistance / speed) + AudioSettings.dspTime}");

        while (!_isHit)
        {
            double currentDspTime = AudioSettings.dspTime;
            // ?¨ì? ?œê°„??ë¹„ë??˜ì—¬ ë§??„ë ˆ???¼ì • ê±°ë¦¬ë§Œí¼ ?´ë™
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);
            transform.position = startPosition + direction * coveredDistance;
            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                Debug.Log($"?¸íŠ¸ê°€ ëª©í‘œ ì§€?ì— ?„ì°©?? ?„ì°© ?œê°„: {currentDspTime}");
                //Destroy(gameObject);
                yield break;
            }
            // ?ŒìŠ¤?¸ìš© ë¡œê·¸ ì¶œë ¥
            //Debug.Log($"?¸íŠ¸ ?´ë™ ì¤?- ?„ì¬ dspTime: {currentDspTime}, ëª©í‘œ ?œê°„: {endDspTime}");
            yield return null;
        }
    }
    /// <summary>
    /// ê³µí†µ???¼ê²© ?ì •???€???ìˆ˜ ì²˜ë¦¬
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
        Debug.Log($"Hit??ê²°ê³¼ : {decision}, ?ìˆ˜ : {scoreValue}");
    }
    /// <summary>
    /// ë²„íŠ¼ ?…ë ¥???°ë¥¸ ?ì • ì²˜ë¦¬
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision);
    /// <summary>
    // ?´í™??ì²˜ë¦¬ (? ë‹ˆë©”ì´???ëŠ” ?Œí‹°??
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("?´í™???™ì‘");
    }
}







