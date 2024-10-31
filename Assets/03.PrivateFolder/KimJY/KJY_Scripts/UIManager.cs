using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{

    public static UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;    
    }

    enum Buttons
    { 
        StartButton,
        StopButton
    }

    enum Texts
    { 
        ScoreText,
        HpText
    }

    enum Images
    {
        PerfectImage,
        GreatImage,
        MissImage
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    void Bind<T>(Type type)
    {
        string[] names = Enum.GetNames(type);
    }

    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _hpBar;


    public void SetProgressValue(float value)
    {
        if (_progressBar.TryGetComponent<IValuableUI>(out IValuableUI ui))
        {
            ui.SetValue(value);
        }
    }

    public void SetHPValue(float value)
    {
        if (_hpBar.TryGetComponent<IValuableUI>(out IValuableUI ui))
        {
            ui.SetValue(value);
        }
    }

}