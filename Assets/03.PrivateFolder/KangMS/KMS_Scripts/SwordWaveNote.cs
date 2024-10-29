using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWaveNote : Note
{
    public Transform testTrransform;

    /// <summary>
    /// 보스의 위치를 목표로 초기화
    /// </summary>
    public void InitializeSwordWave(Transform bossTransform, float speed, float scoreValue, float damage)
    {
        this.testTrransform = bossTransform;
        this.speed = speed;
        this.scoreValue = scoreValue;
        this.damage = damage;

        gameObject.SetActive(true);
        double startDspTime = AudioSettings.dspTime;
        double travelDuration = Vector3.Distance(transform.position, bossTransform.position) / speed;
        double endDspTime = startDspTime + travelDuration;

        StartCoroutine(MoveToLeft(startDspTime, endDspTime));
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InitializeSwordWave(testTrransform, speed, scoreValue, damage);
        }
    }

    /// <summary>
    /// 보스 위치에 따라 지속적으로 목표 지점 갱신
    /// </summary>
    protected override IEnumerator MoveToLeft(double startDspTime, double endDspTime)
    {
        Vector3 startPosition = transform.position;
        float totalDistance = Vector3.Distance(startPosition, endPoint);

        while (!_isHit && isMoving)
        {
            Vector3 currentEndPoint = endPoint;  // 보스 위치 업데이트
            Vector3 direction = (currentEndPoint - startPosition).normalized;

            double currentDspTime = AudioSettings.dspTime;
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);

            transform.position = startPosition + direction * coveredDistance;

            // 보스 위치에 도달하면 노트 비활성화
            if (Vector3.Distance(transform.position, currentEndPoint) <= 0.1f)
            {
                Debug.Log("보스에 도달하여 데미지 전달");
                OnDamage();  // 데미지 전달
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
    public override float OnDamage()
    {
        Debug.Log($"데미지 전달! 데미지 : {damage}");
        return damage;
    }

    public override void OnHit(E_NoteDecision decision)
    {
        _isHit = true;
        CalculateScore(decision);
        ShowEffect();
        gameObject.SetActive(false);
    }

  
}
