using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour, IValuableUI
{
    public Image progressBar;
        
    private void Start()
    {
        SetValue(0);
    }

    public void SetValue(float value)
    { 
        progressBar.fillAmount = value;
    }
}
