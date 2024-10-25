using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_RightCheck : MonoBehaviour
{
    [SerializeField] WGH_JudgeCircle _jucgeCircle;

    private void Start()
    {
        _jucgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _jucgeCircle._right = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _jucgeCircle._right = false;
    }
}
