using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main2 : MonoBehaviour
{
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;
    [SerializeField] private GameObject obj3;

    private void Move()
    {
        obj1.transform.position += Vector3.up * 0.1f;
    }

    private void Rotate()
    {
        obj2.transform.Rotate(Vector3.forward * 3f);
    }

    private void Scale()
    {
        obj3.transform.localScale += Vector3.one * 0.1f;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Move();
            Rotate();
            Scale();
        }
    }
}


