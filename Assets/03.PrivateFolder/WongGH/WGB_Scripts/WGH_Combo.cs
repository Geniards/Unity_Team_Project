using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Combo : MonoBehaviour, IPoolingObj
{
    [SerializeField] SpriteRenderer _sprite;
    public Sprite[] sprites;
    public E_Pool MyPoolType => E_Pool.COMBO;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        transform.position = Vector3.zero;
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
