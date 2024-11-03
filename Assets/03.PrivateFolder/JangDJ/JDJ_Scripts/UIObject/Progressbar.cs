public class Progressbar : Gaugebar
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.Instance.Progressbar = this;
    }

    public override void SetValue(float value)
    {
        _image.fillAmount = value;
    }
}
