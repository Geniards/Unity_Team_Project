using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            transform.position = transform.position + new Vector3(-0.05f, 0, 0);
        }
    }
}
