using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, IManager
{
    private static SceneController _instance = null;
    public static SceneController Instance => _instance;

    private Coroutine _fadeRoutine;
    private WaitForSeconds _fadeSec;
    private E_SceneType _curScene = E_SceneType.EMPTY;

    [SerializeField,Header("FadeIn = 게임 화면이 드러나는 효과")] 
    private List<ScreenFadeList> _screenFadeTimes;

    [SerializeField, Space(10f)] private Image _fadePanelImage;

    public void Init()
    {
        _instance = this;
        _fadeSec = new WaitForSeconds(DataManager.Instance.SceneFadeDuration);
        EventManager.Instance.AddAction(E_Event.STAGE_END, ShowResultScene, this);

        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == (int)E_SceneType.STAGE)
            EventManager.Instance.PlayEvent(E_Event.OPENED_STAGESCENE);
    }

    /// <summary>
    /// 씬 화면이 드러나는 상태로 전환
    /// </summary>
    private IEnumerator FadeInRoutine(float duration)
    {
        float timer = 0;
        Color tempColor = new Color(0, 0, 0, 1);
        _fadePanelImage.color = tempColor;
        float alpha = tempColor.a;
        float t = 0;

        while (true)
        {
            if (t >= 1)
                break;

            timer += Time.deltaTime;
            t = timer / duration;

            tempColor.a = Mathf.Lerp(1, 0, t);
            _fadePanelImage.color = tempColor;

            yield return null;
        }
    }

    /// <summary>
    /// 씬 화면이 가려지는 상태로 전환
    /// </summary>
    private IEnumerator FadeOutRoutine(float duration)
    {
        float timer = 0;
        Color tempColor = new Color(0, 0, 0, 0);
        _fadePanelImage.color = tempColor;
        float alpha = tempColor.a;
        float t = 0;

        while (true)
        {
            if (t >= 1)
                break;

            timer += Time.deltaTime;
            t = timer / duration;

            tempColor.a = Mathf.Lerp(0, 1, t);
            _fadePanelImage.color = tempColor;

            yield return null;
        }
    }

    private IEnumerator FadeFXSceneChange(E_SceneType sceneType)
    {
        float fadeInTime = _screenFadeTimes[(int)sceneType].FadeInTime;
        float fadeOutTime = _screenFadeTimes[(int)_curScene].FadeOutTime;

        yield return FadeOutRoutine(fadeOutTime);

        yield return SceneManager.LoadSceneAsync((int)sceneType);

        yield return FadeInRoutine(fadeInTime);
        
    }

    public void LoadScene(E_SceneType sceneType)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeFXSceneChange(sceneType));
    }

    public void ShowResultScene()
    {
        bool clear = DataManager.Instance.IsStageClear;

        if (clear == true)
        {
            LoadScene(E_SceneType.CLEAR);
        }
        else
        {
            LoadScene(E_SceneType.FAIL);
        }
    }

    

    //private ISceneState CurScene = null;
    //private Dictionary<E_SceneType, ISceneState> _sceneTable = new Dictionary<E_SceneType, ISceneState>();
    //private List<ISceneState> _sceneStatesList = new List<ISceneState>
    //{
    //    { new StartSceneState() },
    //    { new LoadSceneState() },
    //    { new LobbySceneState() },
    //};

    //private void RegistScenesToTable()
    //{
    //    for (int i = 0; i < (int)E_SceneType.E_SCENETYPE_MAX; i++)
    //    {
    //        _sceneTable.Add((E_SceneType)i, _sceneStatesList[i]); 
    //    }
    //}

    //private void Update()
    //{
    //    CurScene.Update();
    //}

    //private void StartSceneFadeInFX()
    //{
    //    if (_fadeRoutine != null)
    //        StopCoroutine(_fadeRoutine);

    //    float duration = _screenFadeTimes[(int)_curScene].FadeInTime;

    //    _fadeRoutine = StartCoroutine(FadeRoutine(duration));
    //}

    //private void StartSceneFadeOutFX()
    //{
    //    if (_fadeRoutine != null)
    //        StopCoroutine(_fadeRoutine);

    //    float duration = _screenFadeTimes[(int)_curScene].FadeOutTime;

    //    _fadeRoutine = StartCoroutine(FadeRoutine(duration));
    //}




}

[System.Serializable]
public class ScreenFadeList
{
    public E_SceneType SceneType;
    public float FadeInTime;
    public float FadeOutTime;
}

//public interface ISceneState
//{
//    public void Enter();
//    public void Update();
//    public void Exit();
//}

//public class StartSceneState : ISceneState
//{
//    public void Enter()
//    {
//    }

//    public void Exit()
//    {
//    }

//    public void Update()
//    {
//        //if(Input.anyKeyDown)
//        //{
//        //    SceneController.Instance.LoadScene(E_SceneType.LOAD);
//        //}
//    }
//}