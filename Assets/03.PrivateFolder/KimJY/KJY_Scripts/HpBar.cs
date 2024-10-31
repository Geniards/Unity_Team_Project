using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour, IValuableUI
{
 
    public Image hpBar;

    private void Start()
    {
        SetValue(200);
    }

    public void SetValue(float value)
    {
        hpBar.fillAmount = value;
    }

    /*
    public float curHp;
    public float initHp;
    public float maxHp = 200;
    public Image hpBar;
        
    private void Start()
    {
        SetValue(200);
    }

    public void SetValue(float value)
    {
        hpBar.fillAmount = value;
    }

    private void Update() 
    {
        hpBar.fillAmount = GetPortion();
    }

    private float GetPortion()
    {
        return curHp / maxHp; 
    }
    */
}
