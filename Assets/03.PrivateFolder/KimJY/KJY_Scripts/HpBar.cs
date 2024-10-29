using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public float curHp;
    public float initHp;
    public float maxHp;
    public Image hpBar;
        
    private void Start()
    {
        curHp = initHp;
    }

    private void Update() // �����Ӹ��� ������Ʈ
    {
        hpBar.fillAmount = GetPortion();
    }

    private float GetPortion()
    {
        return curHp / maxHp; 
    }
}
