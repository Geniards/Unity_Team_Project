using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance => _instance;

    public static NoteDirector NoteDirector;

    private int _stageNumber;
    public int StageNumber => _stageNumber;

    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;

    // TODO 백그라운드 매니저 추가, 정빈님 작업 후 추가 예정

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        InitializeManagers();
        _stageNumber = 1; // 테스트용도
    }

    private void InitializeManagers()
    {
        IManager[] managers = transform.GetComponents<IManager>();

        foreach (var manager in managers)
        {
            manager.Init();
        }
    }

    /// <summary>
    /// 스테이지를 진행시킵니다.
    /// </summary>
    public void StartStage()
    {
        NoteDirector.Initailize();
        NoteDirector.StartSpawnNotes();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartStage();
    }
}
