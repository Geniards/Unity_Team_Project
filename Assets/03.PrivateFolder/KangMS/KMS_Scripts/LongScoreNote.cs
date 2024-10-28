using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongScoreNote : Note
{
    [Header("노트형태")]
    [SerializeField] private Transform head;  
    [SerializeField] private Transform tail;  
    [SerializeField] private Transform body;  
    [SerializeField] private Transform maskLayer;

    private Vector3 _initMaskScale;


    void Update()
    {
        
    }

    public override void OnHit(E_NoteDecision decision)
    {
        
    }

    public override void OnDamage()
    {
    }
}
