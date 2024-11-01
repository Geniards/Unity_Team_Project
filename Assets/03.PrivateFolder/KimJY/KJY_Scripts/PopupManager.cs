using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
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

    private void Awake()
    {
        popupAnimator = popup.GetComponent<Animator>();
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
}
