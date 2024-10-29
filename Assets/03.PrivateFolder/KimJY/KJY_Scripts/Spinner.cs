using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public GameObject targetObject;
    [SerializeField] float rotateSpeed=100f;

    void Update()
    {
        targetObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
