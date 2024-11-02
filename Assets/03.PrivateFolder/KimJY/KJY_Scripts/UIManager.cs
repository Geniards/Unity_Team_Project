using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{
    #region BINDING KEY

    

    #endregion

    public static UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }

    public GameObject ProgressBar;
    public GameObject HpBar;
    
    
    public void SetProgressValue(float value)
    {
        //if (ProgressBar.TryGetComponent<IValuableUI>(out IValuableUI ui))
        //{
        //    ui.SetValue(value);
        //}
    }

    public void SetHPValue(float value)
    {
        //if (HpBar.TryGetComponent<IValuableUI>(out IValuableUI ui))
        //{
        //    ui.SetValue(value);
        //}
    }
}