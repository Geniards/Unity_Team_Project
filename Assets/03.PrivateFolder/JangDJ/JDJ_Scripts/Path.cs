using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private int _pathNumber;
    public int PathNumber => _pathNumber;

    [HideInInspector] public PathPoints Pointer;

    public void SetDest()
    {
        Pointer.SetPosIdx(_pathNumber);
    }
}
