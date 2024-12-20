using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwordWaveNote : Note
{
    [Header("보스의 위치")]
    public Transform curbossTransform;
    private double lastDspTime;
    /// <summary>
    /// 보스의 위치를 목표로 초기화
    /// </summary>
    public void InitializeSwordWave(Transform bossTransform, float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
        this.curbossTransform = DataManager.Instance.Boss.transform;
        if (!curbossTransform) Debug.Log("보스위치가 존재하지 않습니다.");

        AnimatorOverrideController overrideController = GameManager.AnimationChanger.GetAnimationController(0, E_NoteType.SwordWave, E_SpawnerPosY.NONE, 0);
        if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController;
        }
        gameObject.SetActive(true);
        lastDspTime = AudioSettings.dspTime;
    }
    private void Update()
    {
        moveAttackBoss();
    }
    private void moveAttackBoss()
    {
        if (curbossTransform == null || _isHit)
            return;
        // DSP 시간 기반 이동
        double currentDspTime = AudioSettings.dspTime;
        double deltaTime = currentDspTime - lastDspTime;
        lastDspTime = currentDspTime;
        float step = speed * (float)deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, curbossTransform.position, step);
        // 보스 방향으로 회전
        Vector3 directionToBoss = (curbossTransform.position - transform.position).normalized;
        //두 지점 간의 방향을 각도로 변환(Atan사용이유)
        float angle = Mathf.Atan2(directionToBoss.y, directionToBoss.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // 보스 위치에 도달하면 타격 처리
        if (Vector3.Distance(transform.position, curbossTransform.position) < 0.1f)
        {
            DataManager.Instance.Boss.OnDamage(GetDamage());
            Destroy(this.gameObject);
        }
    }
    public override float GetDamage()
    {
        Debug.Log($"데미지 전달! 데미지 : {damage}");
        return damage;
    }
    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {
        _isHit = true;
        ShowEffect();
        gameObject.SetActive(false);
    }
    public override void ReturnToPool() { }
}