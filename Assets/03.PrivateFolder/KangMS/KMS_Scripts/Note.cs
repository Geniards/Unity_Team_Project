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

        Debug.Log($"�߽� :{transform.position}");

        StartCoroutine(MoveToLeft());
    }

    /// <summary>
    /// ���۰� ���ÿ� _endPoint�� ���Ͽ� ���ư����� ����.
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
            
            // �� �����Ӹ��� ������ �ӵ��� �������� �̵�
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
    /// ����� �ǰ� ������ ���� ���� ó��
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
        Debug.Log($"Hit�� ��� : {decision}, ���� : {scoreValue}");
    }

    /// <summary>
    /// ��ư �Է¿� ���� ���� ó��
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision, bool isBoss = false);

    /// <summary>
    // ����Ʈ ó�� (�ִϸ��̼� �Ǵ� ��ƼŬ)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("����Ʈ ����");
    }
}
