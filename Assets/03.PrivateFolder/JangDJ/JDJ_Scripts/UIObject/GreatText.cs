public class GreatText : TextBinder
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.GreatText = this;
    }
}
