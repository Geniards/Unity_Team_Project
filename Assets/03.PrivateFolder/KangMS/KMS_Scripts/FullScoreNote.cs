using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class FullScoreNote : Note
{
    /// <summary>
    /// Test�� Ȯ���ϱ� ���� ���� ���� ���� �ʿ�!
    /// </summary>
    private void Awake()
    {
        // test��
        Initialize(endPoint, speed, scoreValue);
    }

    public override void OnHit(E_NoteDecision decision)
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F) && !_isHit && !isBoss)
        {
            _isHit = true;
            Debug.Log("��ü ������Ʈ Hit!"); 
            CalculateScore(decision);
            Debug.Log("���� : " + scoreValue);
            ShowEffect();
            Destroy(gameObject);
        }
    }
}
