using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeGreatCircleCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("great �浹!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("great ���!");
    }
}
