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
    /// 스테이지를 진행시킵니다.
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

    public void ZoomIn(float duration, float size)
    {
        StartCoroutine(CamZoomRoutine(duration, size));
    }

    private IEnumerator CamZoomRoutine(float duration, float size)
    {
        float time = 0;
        float t = 0;
        float initSize = Camera.main.orthographicSize;

        while (true)
        {
            if (t >= 1)
                break;

            time += Time.deltaTime;
            t = Mathf.Clamp01(time / duration);

            Camera.main.orthographicSize = Mathf.Lerp
                (initSize, size, t);

            yield return null;
        }
    }

    public void CamMove(Vector3 pos, float duration)
    {
        StartCoroutine(CamMoveRoutine(pos, duration));
    }

    private IEnumerator CamMoveRoutine(Vector3 pos, float duration)
    {
        float time = 0;
        float t = 0;
        Vector3 initPos = Camera.main.transform.position;

        while (true)
        {
            if (t >= 1)
                break;

            time += Time.deltaTime;
            t = Mathf.Clamp01(time / duration);

            Camera.main.transform.position
                = Vector3.Lerp(initPos, pos, t);

            yield return null;
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 임시 코드
            StartStage();
    }
}
