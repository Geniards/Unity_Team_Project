using TMPro;
using UnityEngine;

public abstract class TextBinder : MonoBehaviour
{
    private TextMeshProUGUI _text;

    protected virtual void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

    }

    public void SetText(string value)
    {
        _text.text = value;
    }
}
