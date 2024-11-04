using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test_FX : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            EffectManager.Instance.PlayFX(Vector3.zero, E_VFX.MOUSE_INPUT);
    }
}
