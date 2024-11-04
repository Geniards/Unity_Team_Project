public class PerfectText : TextBinder
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.PerfectText = this;
    }
}
