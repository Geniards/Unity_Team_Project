using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private NoteSpawnPosController _posController = null;
    private List<NoteData> _innerNoteList = new List<NoteData>();
    private int _lastNoteIdx = 0;

    public bool IsLastNote
        => _lastNoteIdx >= _innerNoteList.Count - 1;

    private int _patternIdx = 0;

    public void ChangeNotePatter()
    {
        _patternIdx += DataManager.Instance.NormalPatternLastIdx + 1;
    }

    /// <summary>
    /// 스포너가 설정해야 할 값을 초기화 시킵니다.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;
        _patternIdx = 0;

        _innerNoteList = new List<NoteData>();
    }

    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.CHANGED_BGM, Initalize, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, Initalize, this);

        EventManager.Instance.AddAction(E_Event.CHANGED_BGM, RegistPatternData, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, RegistPatternData, this);
    }

    /// <summary>
    /// CSV에 저장된 랜덤 패턴을 불러옵니다.
    /// </summary>
    public void RegistPatternData() // 몇번 패턴인지
    {
        int idx = Random.Range(_patternIdx, _patternIdx + 10);

        Debug.Log($"패턴 번호 {idx}");

        List<NoteData> newNotes =
            DataManager.Instance.CSVData[idx];

        for (int i = 0; i < newNotes.Count; i++)
        {
            _innerNoteList.Add(newNotes[i]);
        }
    }

    /// <summary>
    /// 등록 되어있던 노트 스폰 목록요소를 실체화합니다.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        if (_lastNoteIdx >= _innerNoteList.Count)
        { throw new System.Exception("등록된 노트가 없습니다."); }

        NoteData data = _innerNoteList[_lastNoteIdx];

        //Debug.Log($"{data.position}{(int)(data.noteType)}");

        if (data.noteType == 0)
        {
            _lastNoteIdx++;
            return;
        }

        Note note = GetNoteObject(data.noteType);
        note.transform.position = GetNoteStartPosition(data.position);

        if(data.position-1 == (int)(E_SpawnerPosY.MIDDLE))
        {
            _lastNoteIdx++;
            return;
        }

        note.Initialize(GetNoteEndPosition(data.position), noteSpeed, 2f ,DataManager.Instance.SelectedStageData.StageNumber,data.noteType, (E_SpawnerPosY)data.position-1); // 임시
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
            case E_NoteType.DBScore:
                    return ObjPoolManager.Instance.GetObject<Note>(E_Pool.DBSCORE_NOTE);

        }

        throw new System.Exception("잘못된 노트 요청");
    }

    private Vector3 GetNoteStartPosition(int posNumber)
    {
        switch (posNumber)
        {
            case 3:
                return _posController.GetSpawnerPos(E_SpawnerPosY.TOP);

            case 2:
                return _posController.GetSpawnerPos(E_SpawnerPosY.MIDDLE);

            case 1:
                return _posController.GetSpawnerPos(E_SpawnerPosY.BOTTOM);
        }
        throw new System.Exception("잘못된 노트 위치 요청");
    }

    private Vector3 GetNoteEndPosition(int posNumber)
    {
        switch (posNumber)
        {
            case 3: //top
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.TOP);

            case 2:
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.MIDDLE);

            case 1:
                return _posController.GetSpawnerPos(E_SpawnerPosX.END, E_SpawnerPosY.BOTTOM);
        }

        throw new System.Exception("잘못된 노트 위치 요청");
    }



}
