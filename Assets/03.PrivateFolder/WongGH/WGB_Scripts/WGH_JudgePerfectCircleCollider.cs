using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgePerfectCircleCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("perfect Ãæµ¹!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("perfect ¹þ¾î³²!");
    }
}
