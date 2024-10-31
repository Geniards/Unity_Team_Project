using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance => _instance;

    public static NoteDirector Director;
    public static NoteMediator Mediator;

    private Coroutine _stageTimeRoutine;
    private WaitForSeconds _timerIntervalSec;
    private float _checkInterval = 0.1f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
            DontDestroyOnLoad(_instance.gameObject);

            _instance.InitGameManager();

        }
    }

    private void Awake()
    {
        if (_instance != this)
            Destroy(this.gameObject);
    }

    private void InitGameManager()
    {
        InitializeManagers();
        _timerIntervalSec = new WaitForSeconds(_checkInterval);
        Application.targetFrameRate = 120;
        DataManager.Instance.SetStageNumber(1);
    }

    private void InitializeManagers()
    {
        IManager[] managers = transform.GetComponents<IManager>();

        foreach (var manager in managers)
        { manager.Init(); }
    }

    /// <summary>
    /// 스테이지를 진행시킵니다.
    /// </summary>
    public void StartStage(E_StageBGM bgm)
    {
        DataManager.Instance.SetPlayState(true);
        Director.Initailize();
        Director.StartSpawnNotes(bgm);
        _stageTimeRoutine = StartCoroutine(StartProgressTimer());

        Debug.Log(DataManager.Instance.CurrentBGMClipLength);
        Debug.Log(DataManager.Instance.SkipSpawnTimeOffset);
    }

    /// <summary>
    /// 스테이지 진행시 현재 진행도를 확인하기 위한 코루틴
    /// </summary>
    private IEnumerator StartProgressTimer()
    {
        float timer = 0;
        float breakPoint = DataManager.Instance.CurrentBGMClipLength -
            DataManager.Instance.SkipSpawnTimeOffset;
        //float breakDuration = DataManager.Instance.SkipSpawnTimeOffset / 
        //    DataManager.Instance.CurrentBGMClipLength;
        bool isBreaked = false;

        while (true)
        {
            DataManager.Instance.SetProgress(timer);

            yield return _timerIntervalSec;
            timer += _checkInterval;

            if (timer >= breakPoint && isBreaked == false)
            {
                SoundManager.Instance.FadeBGM(false, 4f);
                Director.SetSpawnSkip(true);
                isBreaked = true;
            }
        }
    }
    
    /// <summary>
    /// 진행시간의 타이머를 종료시킵니다.
    /// </summary>
    public void StopProgressTimer()
    {
        if (_stageTimeRoutine != null)
            StopCoroutine(_stageTimeRoutine);

        SoundManager.Instance.PlayBossBGM();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 임시
            StartStage(E_StageBGM.TEST_NORMAL_01);
    }

    /*
      private void Start()
    {
        SceneManager.sceneLoaded += scene =>
        {
            UIManager.Instance?.BindUIElements();
            UIManager.Instance?.SetPlayerHPValue(DataManager.Instance.PlayerHp);
            UIManager.Instance?.SetProgressValue(DataManager.Instance.StageProgress);
        };
    }
    */

}
