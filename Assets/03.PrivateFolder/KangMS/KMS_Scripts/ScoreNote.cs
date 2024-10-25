using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNote : Note
{
    private void Awake()
    {
        // test¿ë
        Initialize(endPoint, speed, scoreValue);
    }

    public override void OnHit(E_NoteDecision decision, bool isBoss = false)
    {
        if (!_isHit)
        {
            _isHit = true;
            CalculateScore(decision, isBoss);
            ShowEffect();
            Destroy(gameObject);
        }
    }
}
