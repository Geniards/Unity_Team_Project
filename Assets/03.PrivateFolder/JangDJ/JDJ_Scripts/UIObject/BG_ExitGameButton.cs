using UnityEngine;

public class BG_ExitGameButton : BaseButton
{
    public override void MyAction()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
