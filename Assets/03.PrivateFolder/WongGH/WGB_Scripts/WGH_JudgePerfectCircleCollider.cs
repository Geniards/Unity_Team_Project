using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgePerfectCircleCollider : MonoBehaviour
{
    [SerializeField] WGH_JudgeCircle _JudgeCircle;
    private void Start()
    {
        _JudgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Note note))
        {
            _JudgeCircle._isPerfectCircleIn = true;
            //Debug.Log("perfect Ãæµ¹!");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Note note))
        {
            _JudgeCircle._isPerfectCircleIn = false;
            //Debug.Log("perfect ¹þ¾î³²!");
        }
    }
}
