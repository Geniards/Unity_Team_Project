using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_Combo : MonoBehaviour, IPoolingObj
{
    [SerializeField] SpriteRenderer _sprite;
    public Sprite[] sprites;
    private Color _color;
    public E_Pool MyPoolType => E_Pool.COMBO;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        transform.position = Vector3.zero;
    }
    public void ChangeColor(int num)
    {
        if (num > 0 && num <= 49)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#808080", out _color);
            _sprite.color = _color;
        }
        else if (num >= 50 && num <= 99)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#0080FF", out _color);
            _sprite.color = _color;
        }
        else if (num >= 100 && num <= 149)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#00FF80", out _color);
            _sprite.color = _color;
        }
        else if (num >= 150 && num <= 199)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#FFA500", out _color);
            _sprite.color = _color;
        }
        else if (num >= 200)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#FF0000", out _color);
            _sprite.color = _color;
        }
    }
    public void Initialize(Vector3 pos, int num)
    {
        transform.position = pos;
        if(num >= 0 && num < sprites.Length)
        _sprite.sprite = sprites[num];
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(E_Pool.COMBO, this.gameObject);
    }
}
