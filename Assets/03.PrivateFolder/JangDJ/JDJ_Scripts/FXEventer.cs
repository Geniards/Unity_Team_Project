using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXEventer : MonoBehaviour
{
    private FxObject _main;

    private void Start()
    {
        _main = GetComponentInParent<FxObject>();
    }

    public void End()
    {
        _main.Return();
    }
}
