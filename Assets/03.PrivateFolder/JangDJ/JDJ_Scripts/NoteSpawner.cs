using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private NoteSpawnPosController _posController = null;
    private List<NoteData> _innerNoteList = null;
    private int _lastNoteIdx = 0;

    public bool IsLastNote
        => _lastNoteIdx >= _innerNoteList.Count - 1;

    /// <summary>
    /// 스포너가 설정해야 할 값을 초기화 시킵니다.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        _innerNoteList = new List<NoteData>();
    }

    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.CHANGED_BGM, Initalize, this);
        Initalize();
    }

    /// <summary>
    /// 요청받은 패턴 넘버를 기준으로 스폰되어야 할 노트 정보들을 스포너에 등록해둡니다.
    /// </summary>
    public void RegistPattern(int patternNumber) // 몇번 패턴인지
    {
        //List<NotePattern> patterns = CSVLoader.GetPattern(patternNumber);

        //for (int i = 0; i < patterns.Count; i++)
        //{
        //    _innerNoteList.Add(patterns[i]);
        //}

        _innerNoteList.Add(new NoteData(1, E_NoteType.Monster));
        _innerNoteList.Add(new NoteData(3, E_NoteType.Score));
        _innerNoteList.Add(new NoteData(3, E_NoteType.Score));
        _innerNoteList.Add(new NoteData(1, E_NoteType.Obstacle));
    }

    /// <summary>
    /// 등록 되어있던 노트 스폰 목록요소를 실체화합니다.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        if (_lastNoteIdx >= _innerNoteList.Count)
        { throw new System.Exception("등록된 노트가 없습니다."); }

        NoteData data = _innerNoteList[_lastNoteIdx];
        Note note = GetNoteObject(data.noteType);
        note.transform.position = GetNoteStartPosition(data.position);

        note.Initialize(GetNoteEndPosition(data.position), noteSpeed, 10f);
        _lastNoteIdx++;
    }

    private Note GetNoteObject(E_NoteType type)
    {
        switch (type)
        {
            case E_NoteType.None:
                return null;
            case E_NoteType.Score:
                return ObjPoolManager.Instance.GetObject<Note>(E_Pool.SCORE_NOTE);
            case E_NoteType.Monster:
                return ObjPoolManager.Instance.GetObject<Note>(E_Pool.MONSTER_NOTE);
            case E_NoteType.Obstacle:
                return ObjPoolManager.Instance.GetObject<Note>(E_Pool.OBSTACLE_NOTE);
            case E_NoteType.ConcurrentScore:
                    return ObjPoolManager.Instance.GetObject<Note>(E_Pool.DBSCORE_NOTE);

        }

        throw new System.Exception("잘못된 노트 요청");
    }

    private Vector3 GetNoteStartPosition(int posNumber)
    {
        switch (posNumber)
        {
            case 1:
                return _posController.GetSpawnerPos(E_SpawnerPosY.TOP);

            case 2:
                return _posController.GetSpawnerPos(E_SpawnerPosY.MIDDLE);

            case 3:
                return _posController.GetSpawnerPos(E_SpawnerPosY.BOTTOM);
        }
        throw new System.Exception("잘못된 노트 위치 요청");
    }

    private Vector3 GetNoteEndPosition(int posNumber)
    {
        switch (posNumber)
        {
            case 1:
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.TOP);

            case 2:
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.MIDDLE);

            case 3:
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.BOTTOM);
        }

        throw new System.Exception("잘못된 노트 위치 요청");
    }



}
