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
    private E_SceneType _curScene = E_SceneType.NONE;

    [SerializeField,Header("FadeIn = 화면이 가려지는효과")] 
    private List<ScreenFadeList> _screenFadeTimes;

    [SerializeField, Space(10f)] private Image _fadePanelImage;

    //private ISceneState CurScene = null;
    //private Dictionary<E_SceneType, ISceneState> _sceneTable = new Dictionary<E_SceneType, ISceneState>();
    //private List<ISceneState> _sceneStatesList = new List<ISceneState>
    //{
    //    { new StartSceneState() },
    //    { new LoadSceneState() },
    //    { new LobbySceneState() },
    //};

    public void Init()
    {
        _instance = this;
        _fadeSec = new WaitForSeconds(DataManager.Instance.SceneFadeDuration);
        //RegistScenesToTable();
        //CurScene = _sceneTable[E_SceneType.START];

        //EventManager.Instance.AddAction(E_Event.STAGE_END, StartSceneFadeOutFX, this);
        //EventManager.Instance.AddAction(E_Event.CHANGED_SCENE, StartSceneFadeInFX, this); // 씬 매니저 담당
    }

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

    /// <summary>
    /// fadeType == true, 검은안개가 사라지는 루틴
    /// </summary>
    private IEnumerator FadeInRoutine(float duration)
    {
        float timer = 0;
        Color color = new Color(1, 1, 1, 1);
        float alpha = color.a;

        while (true)
        {
            timer += Time.deltaTime;
            float t = timer / duration;



            yield return null;
        }
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        // ui 요소로 연출

        yield break;
    }

    private IEnumerator FadeFXSceneChange(E_SceneType sceneType)
    {
        float fadeInTime = _screenFadeTimes[(int)_curScene].FadeInTime;
        float fadeOutTime = _screenFadeTimes[(int)sceneType].FadeOutTime;

        yield return FadeOutRoutine(fadeOutTime);

        yield return SceneManager.LoadSceneAsync((int)sceneType + 1);

        yield return FadeInRoutine(fadeInTime);
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

    public void LoadScene(E_SceneType sceneType)
    {
        SceneManager.LoadScene((int)sceneType+1);
    }


}

[System.Serializable]
public class ScreenFadeList
{
    public E_SceneType SceneType;
    public float FadeOutTime;
    public float FadeInTime;
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