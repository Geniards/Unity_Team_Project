using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public void OnClickTrigger()
    {
        PopupManager.Instance.OpenPopup
        (
        () => {Debug.Log("Close!!");},
        () => {Debug.Log("Tutorial!!");},
        () => {Debug.Log("Exit!!");}
        );
    }
}
