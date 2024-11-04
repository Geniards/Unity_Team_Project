using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_ExitButton : BaseButton
{
    public override void MyAction()
    {
        SceneController.Instance.LoadScene(E_SceneType.LOBBY);
    }
}
