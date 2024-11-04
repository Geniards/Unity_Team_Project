using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
public abstract class Note : MonoBehaviour
{
    [Header("노트 세부 조정")]
    public float speed = 5f;
    public float damage = 0;
    public Vector3 endPoint;
    public bool _isHit = false;
    public static bool isBoss = false;   //false가 기본값

    [Header("애니메이션 세팅")]
    public Animator animator;

    public virtual void Initialize(Vector3 endPoint, float speed, int stageNumber = 1, E_NoteType noteType = E_NoteType.None, E_SpawnerPosY notePosition = E_SpawnerPosY.BOTTOM, float damage = 0)

    {
        _isHit = false;
        gameObject.SetActive(true);
        this.endPoint = endPoint;
        this.speed = speed;
        // Note 생성 시 중재자에 등록
        GameManager.Mediator.Register(this);
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        // AnimationManager를 통해 개별적인 AnimatorOverrideController 생성 및 설정
        AnimatorOverrideController overrideController = GameManager.AnimationChanger.GetRandomAnimationController(noteType, notePosition, stageNumber);
        if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController;
        }
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
        while (!_isHit && Vector3.Distance(transform.position, endPoint) > 0.001f)
        {
            double currentDspTime = AudioSettings.dspTime;
            // 남은 시간에 비례하여 매 프레임 일정 거리만큼 이동
            double elapsedTime = currentDspTime - startDspTime;
            float coveredDistance = Mathf.Min((float)(elapsedTime * speed), totalDistance);
            transform.position = startPosition + direction * coveredDistance;
            if (Vector3.Distance(transform.position, endPoint) <= 0.001f)
            {
                GameManager.Mediator.Unregister(this);
                ReturnToPool();
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 버튼 입력에 따른 판정 처리
    /// </summary>
    public abstract void OnHit(E_NoteDecision decision, E_Boutton button = E_Boutton.None);

    /// <summary>
    /// Damage 전달 메서드
    /// </summary>
    public abstract float GetDamage();

    public void SetDamage(float damage) { this.damage = damage; }

    /// <summary>
    /// 오브젝트 풀로 반환하는 메서드
    /// </summary>
    public abstract void ReturnToPool();

    /// <summary>
    // 이펙트 처리 (애니메이션 또는 파티클)
    /// </summary>
    protected void ShowEffect()
    {
        Debug.Log("이펙트 동작");
    }
}