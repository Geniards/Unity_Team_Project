public class HomeButton : BaseButton
{
    public override void MyAction()
    {
        SceneController.Instance.LoadScene(E_SceneType.LOBBY);
        SoundManager.Instance.FadeChangeBGM(E_MainBGM.LOBBY);
    }
}


