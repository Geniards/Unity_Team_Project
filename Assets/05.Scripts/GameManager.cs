using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance => _instance;

    public static NoteDirector NoteDirector;

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
    /// ���������� ���α���� ���۽�ŵ�ϴ�.
    /// </summary>
    public void StartStage()
    {
        DataManager.Instance.SetPlayState(true);

        if (NoteDirector == null)
        {
            return;
        }

        NoteDirector.Initailize();
        NoteDirector.StartSpawnNotes();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ?�시 코드
            StartStage();
    }
}
