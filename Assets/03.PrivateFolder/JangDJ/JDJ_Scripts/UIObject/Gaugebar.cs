using UnityEngine;
using UnityEngine.UI;

public abstract class Gaugebar : MonoBehaviour
{
    protected Image _image;

    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
    }

    public abstract void SetValue(float value);
}