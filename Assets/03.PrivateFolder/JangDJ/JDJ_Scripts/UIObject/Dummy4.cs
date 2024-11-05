using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy4 : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.FadeChangeBGM(E_MainBGM.STAGE_FAIL, 0.25f, 0);      
    }
}
