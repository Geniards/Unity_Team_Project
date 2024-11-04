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

    public const string LOBBY_IDX_KEY = "LobbyIDX";

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
    public WGH_ScoreBoard CurScoreText;

    public Progressbar Progressbar;
    public HPbar HpBar;
    
    public void SetCurrentScoreText(string value)
    {
        CurScoreText.SetText($"Score {value}");
    }
    public void SetProgressValue(float value) { Progressbar.SetValue(value); }
    public void SetHPValue(float value) { HpBar.SetValue(value); }
    public void SetTimeValue(float totalTime) { TimeText.SetText(totalTime.ToString()); }

    public void UpdateTexts()
    {
        TimeText.SetText(DataManager.Instance.StagePlayTime.ToString());
        PerfectText.SetText(DataManager.Instance.PerfectCount.ToString());
        GreatText.SetText(DataManager.Instance.GreatCount.ToString());
        ScoreText.SetText(DataManager.Instance.CurScore.ToString());
    }
}