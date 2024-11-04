using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSetter : MonoBehaviour
{
    private void Start()
    {
        Collider2D[] colls = GetComponentsInChildren<Collider2D>(true);

        foreach (var coll in colls)
        {
            coll.enabled = false;
        }
    }
}
