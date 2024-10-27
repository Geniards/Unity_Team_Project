using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongScoreNote : Note
{
    [SerializeField] private float noteDuration = 2.0f;
    private bool _isHolding = false;
    private float _holdStartTime;
    private bool _isFailed = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.F))
        {
            _isHolding = true;
            if (!_isHit)
            {
                //OnHit();
            }
        }
        else
        {
            // 버튼을 떼면 실패
            if (_isHolding)
            {
                _isFailed = true;
                FailNote();
            }
            _isHolding = false;
        }
    }

    public override void OnHit(E_NoteDecision decision, bool isBoss = false)
    {
        if (!_isFailed)
        {
            _isHit = true;
            _holdStartTime = Time.time;
            StartCoroutine(HoldNoteCoroutine());
        }
    }

    // 버튼을 길게 누르는 동안 점수 획득 처리
    private IEnumerator HoldNoteCoroutine()
    {
        float elapsedTime = 0f;

        while (_isHolding && !_isFailed && elapsedTime < noteDuration)
        {
            elapsedTime = Time.time - _holdStartTime;

            // 지속적으로 점수 획득
            Debug.Log("긴 점수 노트 점수획득 중");
            ShowEffect();

            yield return null;
        }

        if (!_isFailed && elapsedTime >= noteDuration)
        {
            // 노트가 끝까지 도달한 경우
            Debug.Log("긴 점수 노트 완료");
            Destroy(gameObject);
        }
    }

    private void FailNote()
    {
        Debug.Log("긴 점수 노트 실패");
        Destroy(gameObject);
    }
}
