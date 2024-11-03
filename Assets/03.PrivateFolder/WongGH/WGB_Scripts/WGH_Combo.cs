using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Combo : MonoBehaviour, IPoolingObj
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] WGH_AreaJudge _judge;
    public Sprite[] sprites;
    public E_Pool MyPoolType => E_Pool.COMBO;
    private float _time;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _judge = FindAnyObjectByType<WGH_AreaJudge>();
    }

    private void Update()
    {
        ChangeNumber();
    }

    public void ChangeNumber()
    {
        if(_judge == null)
        { return; }
        
        if (_judge.Combo == 0)
        {
            _sprite.transform.position = new Vector3(0, 0, 0);
            _sprite.sprite = sprites[0];
        }
        else if (_judge.Combo / 10 < 1)
        {
            _sprite.transform.position = new Vector3(0, 0, 0);
            _sprite.sprite = sprites[_judge.Combo];
        }
    }


    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(E_Pool.COMBO, this.gameObject);
    }
}
