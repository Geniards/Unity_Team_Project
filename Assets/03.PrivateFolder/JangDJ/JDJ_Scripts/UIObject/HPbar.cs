public class HPbar : Gaugebar
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.HpBar = this;
    }

    public override void SetValue(float value)
    {
        _image.fillAmount = value;
    }
}
