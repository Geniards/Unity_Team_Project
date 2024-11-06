public class TimeText : TextBinder
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.TimeText = this;
    }

}
