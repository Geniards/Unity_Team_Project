using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_FloatCombo : MonoBehaviour
{
    [SerializeField] WGH_AreaJudge _judge;

    private void Start()
    {
        _judge = GetComponent<WGH_AreaJudge>();
    }

    public void SpawnCombo(int comboNum)
    {
        SpriteRenderer sprite = ObjPoolManager.Instance.GetObject<SpriteRenderer>(E_Pool.COMBO);
    }
}
