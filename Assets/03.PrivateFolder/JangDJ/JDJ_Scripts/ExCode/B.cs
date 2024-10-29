using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.CLICK, Rotate, this);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * 3);
    }
}
