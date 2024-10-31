using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySc : MonoBehaviour
{
    public void OnClickMyButton()
    {
        UI_Popup.instance.OpenPopup
        (
        () => {Debug.Log("Close!!");},
        () => {Debug.Log("Tutorial!!");},
        () => {Debug.Log("Exit!!");}
        );
    }
}
