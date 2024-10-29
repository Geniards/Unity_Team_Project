using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main1 : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            EventManager.Instance.PlayEvent(E_Event.CLICK);
    }
}
