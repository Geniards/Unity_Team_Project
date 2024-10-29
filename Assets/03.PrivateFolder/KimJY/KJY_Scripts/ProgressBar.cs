using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float curProgress;
    public float initProgress;
    public float maxProgress;
    public Image progressBar;
        
    private void Start()
    {
        if (DataManager.Instance != null)
        {
            curProgress = DataManager.Instance.StageProgress;
        }
        else
        {
            Debug.LogWarning("DataManager.Instance가 초기화되지 않았습니다. initProgress로 초기화합니다.");
            curProgress = initProgress;
        }
    }

    private void Update() 
    {
        progressBar.fillAmount = GetPrgPortion();
    }

    private float GetPrgPortion()
    {
        return curProgress / maxProgress; 
    }
}
