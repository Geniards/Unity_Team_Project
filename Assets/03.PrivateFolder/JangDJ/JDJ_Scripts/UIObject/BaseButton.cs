using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] protected Button _button;

    private void Start()
    {
        _button.onClick.AddListener(MyAction);
    }

    public abstract void MyAction();
}