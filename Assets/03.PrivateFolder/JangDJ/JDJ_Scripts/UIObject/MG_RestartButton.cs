public class MG_RestartButton : BaseButton
{
    public override void MyAction()
    {
        SceneController.Instance.LoadScene(E_SceneType.STAGE);
    }
}
