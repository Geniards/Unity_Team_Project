using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance => _instance;

    public static NoteDirector NoteDirector;

    private Coroutine _stageTimeRoutine;
    private WaitForSeconds _timerIntervalSec;

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
        _timerIntervalSec = new WaitForSeconds(1f);
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
        NoteDirector.Initailize();
        NoteDirector.StartSpawnNotes(bgm);
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
        bool isBreaked = false;

        while (true)
        {
            DataManager.Instance.SetProgress(timer);

            yield return _timerIntervalSec;
            timer += Time.deltaTime;

            if (timer >= breakPoint && isBreaked)
            {
                NoteDirector.SetSpawnSkip(true);
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 임시
            StartStage(E_StageBGM.NORMAL_1);
    }

}
