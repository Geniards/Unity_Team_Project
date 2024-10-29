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
        if (DataManager.Instance != null)
        {
            curHp = DataManager.Instance.PlayerHp;
        }
        else
        {
            Debug.LogWarning("DataManager.Instance가 초기화되지 않았습니다. initHp로 초기화합니다.");
            curHp = initHp;
        }
    }

    private void Update() 
    {
        hpBar.fillAmount = GetPortion();
    }

    private float GetPortion()
    {
        return curHp / maxHp; 
    }
}
