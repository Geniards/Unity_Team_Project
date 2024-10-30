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
    public void StartStage()
    {
        DataManager.Instance.SetPlayState(true);
        NoteDirector.Initailize();
        NoteDirector.StartSpawnNotes();
        _stageTimeRoutine = StartCoroutine(StartTimer());
    }

    /// <summary>
    /// 스테이지 진행시 현재 진행도를 확인하기 위한 코루틴
    /// </summary>
    private IEnumerator StartTimer()
    {
        float Timer = 0;

        while (true)
        {
            DataManager.Instance.SetProgress(Timer);

            yield return _timerIntervalSec;
            Timer += Time.deltaTime;
        }
    }
    
    private void StopTimer() // 스테이지 종료시 반드시 호출
    {
        if (_stageTimeRoutine != null)
            StopCoroutine(_stageTimeRoutine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 임시
            StartStage();
    }

}
