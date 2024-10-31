using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    public GameObject popup;
    Animator popupAnimator;

    public static UI_Popup instance { get; private set; }

    Action onClickClose, onClickTutorial, onClickExit;

    public override void Init()
    {
        //UIManager.SetCanvas(gameObject, true);
    }

    private void Awake()
    {
        instance = this;
        popupAnimator = GetComponent<Animator>();
    }

    public void OpenPopup(Action onClickClose, Action onClickTutorial, Action onClickExit)
    { 
        this.onClickClose = onClickClose;
        this.onClickTutorial = onClickTutorial;
        this.onClickExit = onClickExit;
        popup.SetActive(true);
    }

    public void OnClickClose()
    {
        if (onClickClose != null)
        {
            onClickClose();
        }
        ClosePopup();
    }

    public void OnClickTutorial()
    {
        if (onClickTutorial != null)
        {
            onClickTutorial();
        }
        ClosePopup();
    }

    public void OnClickExit()
    {
        if(onClickExit != null)
        {
            onClickExit();
        }
        ClosePopup();
    }

    void ClosePopup() 
    {
        popupAnimator.SetTrigger("close");
    }

   

    /*
    public virtual void ClosePopup()
    {
        //UIManager.ClosePopup(this);
    }

    */
}
