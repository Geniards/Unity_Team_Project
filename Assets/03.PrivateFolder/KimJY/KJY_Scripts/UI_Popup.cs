using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    public GameObject popup;

    public static UI_Popup instance { get; private set; }

    public Text txtTitle, txtContent;
    Action onClickOK, onClickCancel;

    private void Awake()
    {
        instance = this;
    }

    public void OpenPopup(string title, string content, Action onClickOK, Action onClickCancel)
    { 
        txtTitle.text= title;
        txtContent.text = content;
        this.onClickOK = onClickOK;
        this.onClickCancel = onClickCancel;
    }

    public void OnClickOK()
    {
        if (onClickOK != null)
        {
            onClickOK();
        }
        ClosePopup();
    }

    public void OnClickCancel()
    {
        if(onClickCancel != null)
        {
            onClickCancel();
        }
        ClosePopup();
    }

    void ClosePopup() { }

    public override void Init()
    {
        //UIManager.SetCanvas(gameObject, true);
    }

    /*
    public virtual void ClosePopup()
    {
        //UIManager.ClosePopup(this);
    }
    */
}
