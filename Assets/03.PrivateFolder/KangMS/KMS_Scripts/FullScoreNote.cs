using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class FullScoreNote : Note
{
    public override void OnHit(E_NoteDecision decision, bool isBoss = false)
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F) && !_isHit && !isBoss)
        {
            _isHit = true;
            Debug.Log("��ü ������Ʈ Hit!"); 
            CalculateScore(decision, isBoss);
            Debug.Log("���� : " + scoreValue);
            ShowEffect();
            Destroy(gameObject);
        }
    }
}
