using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.CLICK,Move);
    }

    private void Move()
    {
        transform.position += Vector3.up * 0.1f;
    }
}
