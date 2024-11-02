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

    #region 스테이지 씬
    public GameObject ProgressBar;
    public GameObject HpBar;
    #endregion

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