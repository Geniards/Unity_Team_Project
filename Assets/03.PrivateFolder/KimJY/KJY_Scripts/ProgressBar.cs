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
            Debug.LogWarning("DataManager.Instance�� �ʱ�ȭ���� �ʾҽ��ϴ�. initProgress�� �ʱ�ȭ�մϴ�.");
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
