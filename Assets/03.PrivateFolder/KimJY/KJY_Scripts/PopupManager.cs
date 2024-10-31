using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : UI_Base
{
    private static PopupManager _instance;
    public static PopupManager Instance
    { 
        get 
        { 
            return _instance; 
        } 
    }

    public GameObject popup;
    private Animator popupAnimator;

    private Action _OnClickClose;
    private Action _OnClickTutorial;
    private Action _OnClickExit;


    public override void Init()
    {

    }

    private void Awake()
    {
        popup.SetActive(false);
        DontDestroyOnLoad(this);
        _instance = this;
    }

    public void OpenPopup(Action OnClickClose, Action OnClickTutorial, Action OnClickExit)
    {
        popup.SetActive(true);
        _OnClickClose = OnClickClose;
        _OnClickTutorial = OnClickTutorial;
        _OnClickExit =OnClickExit;
    }

    void ClosePopup()
    {
        popupAnimator.SetTrigger("close");
    }

    public void OnClickClose()
    {
        if (_OnClickClose != null)
        {
            Debug.Log("닫힘버튼 누름");
            _OnClickClose();
        }
        ClosePopup();
    }

    public void OnClickTutorial()
    {
        if (_OnClickTutorial != null)
        {
            Debug.Log("튜토리얼버튼 누름");
            _OnClickTutorial();
        }
        ClosePopup();
    }

    public void OnClickExit()
    {
        if (_OnClickExit != null)
        {
            Debug.Log("나가기버튼 누름");
            _OnClickExit();
        }
        ClosePopup();
    }

    /*
    public GameObject popup;
    private Animator popupAnimator;

    public static UI_Popup instance { get; private set; }

    Action onClickClose, onClickTutorial, onClickExit;

    public override void Init()
    {
        //UIManager.SetCanvas(gameObject, true);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

    public virtual void ClosePopup()
    {
        //UIManager.ClosePopup(this);
    }

    */
}
