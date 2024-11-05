using UnityEngine;

public class RestartButton : BaseButton
{
    public override void MyAction()
    {
        SceneController.Instance.LoadScene(E_SceneType.STAGE);
    }
}
