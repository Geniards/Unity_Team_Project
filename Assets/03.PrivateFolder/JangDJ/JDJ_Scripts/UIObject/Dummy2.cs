using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy2 : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.PlayEvent(E_Event.OPENED_CLEARSCENE);
    }
}
