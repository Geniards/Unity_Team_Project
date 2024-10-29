using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class FullScoreNote : Note
{
    public override void OnDamage()
    {
        
    }

    public override void OnHit(E_NoteDecision decision)
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F) && !_isHit && !isBoss)
        {
            _isHit = true;
            Debug.Log("전체 점수노트 Hit!"); 
            CalculateScore(decision);
            Debug.Log("점수 : " + scoreValue);
            ShowEffect();
            Destroy(gameObject);
        }
    }
}
