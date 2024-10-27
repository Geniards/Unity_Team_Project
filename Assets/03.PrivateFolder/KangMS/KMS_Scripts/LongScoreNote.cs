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
            // ��ư�� ���� ����
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

    // ��ư�� ��� ������ ���� ���� ȹ�� ó��
    private IEnumerator HoldNoteCoroutine()
    {
        float elapsedTime = 0f;

        while (_isHolding && !_isFailed && elapsedTime < noteDuration)
        {
            elapsedTime = Time.time - _holdStartTime;

            // ���������� ���� ȹ��
            Debug.Log("�� ���� ��Ʈ ����ȹ�� ��");
            ShowEffect();

            yield return null;
        }

        if (!_isFailed && elapsedTime >= noteDuration)
        {
            // ��Ʈ�� ������ ������ ���
            Debug.Log("�� ���� ��Ʈ �Ϸ�");
            Destroy(gameObject);
        }
    }

    private void FailNote()
    {
        Debug.Log("�� ���� ��Ʈ ����");
        Destroy(gameObject);
    }
}
