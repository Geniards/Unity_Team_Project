public class WGH_ScoreBoard : TextBinder
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.CurScoreText = this;
        _text.text = "Score";
    }
}
