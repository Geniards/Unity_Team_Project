using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.CLICK, Scale);
    }

    public void Scale()
    {
        transform.localScale += Vector3.one * 0.1f;
    }
}
