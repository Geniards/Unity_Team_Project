using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, IManager
{
    private static SceneController _instance = null;
    public static SceneController Instance => _instance;

    private Coroutine _fadeRoutine;
    private WaitForSeconds _fadeSec;

    public void Init()
    {
        _instance = this;
        _fadeSec = new WaitForSeconds(DataManager.Instance.SceneFadeDuration);
        EventManager.Instance.AddAction(E_Event.STAGE_END, StartSceneFadeOutFX, this);
        EventManager.Instance.AddAction(E_Event.CHANGED_SCENE, StartSceneFadeInFX, this); // 씬 매니저 담당
    }

    private void StartSceneFadeInFX()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(true));
    }

    private void StartSceneFadeOutFX()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(false));
    }

    /// <summary>
    /// fadeType == true, 검은안개가 사라지는 루틴
    /// </summary>
    private IEnumerator FadeRoutine(bool fadeType)
    {
        // ui 요소로 연출

        yield break;
    }

    public void ShowResultScene()
    {
        bool clear = DataManager.Instance.IsStageClear;

        if(clear == true)
        { 
            //SceneManager.LoadScene(성공씬)        
        }
        else
        {
            //SceneManager.LoadScene(실패씬)
        }
    }
}
