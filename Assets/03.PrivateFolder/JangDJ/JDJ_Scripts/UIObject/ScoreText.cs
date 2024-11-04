public class ScoreText : TextBinder
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.ScoreText = this;
    }
}
