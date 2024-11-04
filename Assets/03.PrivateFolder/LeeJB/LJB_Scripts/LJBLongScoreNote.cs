using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBLongScoreNote : Note
{
    [Header("노트 파트 배열")]
    [SerializeField] private GameObject[] noteParts;  // 노트를 구성하는 파트 배열

    private bool _isTouching = false;
    private int currentPartIndex = 0;

    private void Awake()
    {
        Initialize(endPoint, speed, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnHit(E_NoteDecision.Perfect, E_Boutton.None);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            ReleaseTouch();
        }

        // 프레임마다 파트 제거
        DestroyNoteParts();
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton boutton)
    {
        if (!_isTouching)
        {
            _isTouching = true;
        }
    }

    private void DestroyNoteParts()
    {
        // 터치 상태이고 삭제해야 할 파트가 남아 있을 경우
        if (_isTouching && currentPartIndex < noteParts.Length)
        {
            Destroy(noteParts[currentPartIndex]);  // 순차적으로 파트 삭제
            currentPartIndex++;

            // 모든 파트를 제거한 경우 처리
            if (currentPartIndex >= noteParts.Length)
            {
                gameObject.SetActive(false);
            }
        }
        else if (!_isTouching && currentPartIndex < noteParts.Length)
        {
            Debug.Log("롱 노트 실패");
            Destroy(gameObject);
        }
    }

    public void ReleaseTouch()
    {
        _isTouching = false;
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override void ReturnToPool()
    {
        // NoteMediator.Instance.Unregister(this); // 중재자에서 노트 제거
        // Return();
    }
}
