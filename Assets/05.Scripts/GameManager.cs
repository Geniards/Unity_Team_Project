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

    // TODO ��׶��� �Ŵ��� �߰�, ����� �۾� �� �߰� ����

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
        _stageNumber = 1; // �׽�Ʈ�뵵
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
    /// ���������� �����ŵ�ϴ�.
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
