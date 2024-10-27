using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    [Header("��Ʈ ���� ����")]
    public float speed = 5f;
    public float scoreValue = 100;
    public Vector3 endPoint;

    [Header("�浹 ������ ��Ʈ ��/��")]
    public bool _isHit = false;
    [Header("���� �⿬ ��/��")]
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
    /// ���۰� ���ÿ� _endPoint�� ���Ͽ� ���ư����� ����.
    /// </summary>
    protected virtual IEnumerator MoveToLeft(double startDspTime, double endDspTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (endPoint - startPosition).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPoint);

        while (!_isHit)
        {
            double currentDspTime = AudioSettings.dspTime;

            // ���� �ð��� ����Ͽ� �� ������ ���� �Ÿ���ŭ �̵�
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);

            transform.position = startPosition + direction * coveredDistance;


            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                Debug.Log($"��Ʈ�� ��ǥ ������ ������, ���� �ð�: {currentDspTime}");

                Destroy(gameObject);
                yield break;
            }

            // �׽�Ʈ�� �α� ���
            Debug.Log($"��Ʈ �̵� �� - ���� dspTime: {currentDspTime}, ��ǥ �ð�: {endDspTime}");

            yield return null;
        }
    }

    /// <summary>
    /// ����� �ǰ� ������ ���� ���� ó��
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
        Debug.Log($"Hit�� ��� : {decision}, ���� : {scoreValue}");
    }

    /// <summary>
    /// ��ư �Է¿� ���� ���� ó��
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision);

    /// <summary>
    // ����Ʈ ó�� (�ִϸ��̼� �Ǵ� ��ƼŬ)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("����Ʈ ����");
    }
}
