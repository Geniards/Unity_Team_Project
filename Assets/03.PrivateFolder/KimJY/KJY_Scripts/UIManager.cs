using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        EventManager.Instance.AddAction(E_Event.OPENED_CLEARSCENE, UpdateTexts, this);
    }

    public TimeText TimeText;
    public PerfectText PerfectText;
    public GreatText GreatText;
    public ScoreText ScoreText;

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

    public void UpdateTexts()
    {
        // 타임 추가
        PerfectText.SetText(DataManager.Instance.PerfectCount.ToString());
        GreatText.SetText(DataManager.Instance.GreatCount.ToString());
        ScoreText.SetText(DataManager.Instance.CurScore.ToString());
    }
}