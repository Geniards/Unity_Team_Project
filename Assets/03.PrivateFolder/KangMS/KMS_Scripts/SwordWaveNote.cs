using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWaveNote : Note
{
    public Transform testTrransform;
    private bool a = false; // test변수
    private double lastDspTime;

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
        lastDspTime = AudioSettings.dspTime;
        // test변수
        a = true;
    }
    private void Update()
    {
        // test
        if (a)
        {
            moveBoss();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            testTrransform.position = new Vector3(7, -4, 0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            testTrransform.position = new Vector3(7, 0, 0);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            testTrransform.position = new Vector3(7, 4, 0);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            testTrransform.position = new Vector3(0, 0, 0);
    }

    private void moveBoss()
    {
        if (testTrransform == null || _isHit)
            return;

        // DSP 시간 기반 이동
        double currentDspTime = AudioSettings.dspTime;
        double deltaTime = currentDspTime - lastDspTime;
        lastDspTime = currentDspTime;

        // 이동 계산: deltaTime을 사용하여 속도에 따라 이동
        float step = speed * (float)deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, testTrransform.position, step);

        // 보스 위치에 도달하면 타격 처리
        if (Vector3.Distance(transform.position, testTrransform.position) < 0.1f)
        {
            HitBoss();
        }
    }

    private void HitBoss()
    {
        Debug.Log("보스에게 데미지를 입혔습니다!");
        gameObject.SetActive(false);
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
