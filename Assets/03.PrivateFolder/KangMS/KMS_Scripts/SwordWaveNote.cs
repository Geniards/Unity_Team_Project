using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWaveNote : Note
{
    [Header("보스의 위치")]
    public Transform bossTrransform;
    private double lastDspTime;

    /// <summary>
    /// 보스의 위치를 목표로 초기화
    /// </summary>
    public void InitializeSwordWave(float speed, float scoreValue, float damage)
    {
        this.speed = speed;
        this.scoreValue = scoreValue;
        this.damage = damage;

        if (!bossTrransform) Debug.Log("보스위치가 존재하지 않습니다.");

        gameObject.SetActive(true);
        lastDspTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        moveBoss();
    }

    private void moveBoss()
    {
        if (bossTrransform == null || _isHit)
            return;

        // DSP 시간 기반 이동
        double currentDspTime = AudioSettings.dspTime;
        double deltaTime = currentDspTime - lastDspTime;
        lastDspTime = currentDspTime;

        // 이동 계산: deltaTime을 사용하여 속도에 따라 이동
        float step = speed * (float)deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, bossTrransform.position, step);

        // 보스 방향으로 회전
        Vector3 directionToBoss = (bossTrransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToBoss);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);

        // 보스 위치에 도달하면 타격 처리
        if (Vector3.Distance(transform.position, bossTrransform.position) < 0.1f)
        {
            HitBoss();
        }
    }

    private void HitBoss()
    {
        Debug.Log("보스에게 데미지를 입혔습니다!");
        gameObject.SetActive(false);
    }

    public override float GetDamage()
    {
        Debug.Log($"데미지 전달! 데미지 : {damage}");
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {
        _isHit = true;
        CalculateScore(decision);
        ShowEffect();
        gameObject.SetActive(false);
    }

    public override void ReturnToPool() { }
}
