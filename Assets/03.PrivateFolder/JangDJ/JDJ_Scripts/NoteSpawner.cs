using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private NoteSpawnPosController _posController = null;
    private List<NotePattern> _innerNoteList = null;
    private int _lastNoteIdx = 0;

    public bool IsLastNote
        => _lastNoteIdx == _innerNoteList.Count - 1;

    //// 기능 테스트용
    [SerializeField,Space(20f),Header("기능테스트용")] 
    private Note _notePrefab = null;


    ////

    /// <summary>
    /// 스포너가 설정해야 할 값을 초기화 시킵니다.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        _innerNoteList = new List<NotePattern>();
    }

    private void Start()
    {
        Initalize();
    }

    /// <summary>
    /// 요청받은 패턴 넘버를 기준으로 스폰되어야 할 노트 정보들을 스포너에 등록해둡니다.
    /// </summary>
    //public void RegistPattern(int patternNumber) // 몇번 패턴인지
    //{
    // 해당 CSV 로 부터 해당 패턴의 정보를 불러온다.
    // NotePattern 리스트 덩어리를 불러와서 자체를 복사 진행

    //_innerNoteList.Clear(); //  매번 삭제 부담스러움, 인덱스를 사용한다.

    //    List<NotePattern> patterns = CSVLoader.GetPattern(patternNumber);

    //    for (int i = 0; i < patterns.Count; i++)
    //    {
    //        _innerNoteList.Add(patterns[i]);
    //    }
    //}

    /// <summary>
    /// 등록 되어있던 노트 스폰 목록요소를 실체화합니다.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        if (_lastNoteIdx >= _innerNoteList.Count)
        { throw new System.Exception("등록된 노트가 없습니다."); }

        NotePattern pattern = _innerNoteList[_lastNoteIdx];
        Note note = GetNoteObject(pattern.noteType);
        note.transform.position = GetNoteStartPosition(pattern.position);

        note.Initialize(GetNoteEndPosition(pattern.position), noteSpeed, 10f);
        _lastNoteIdx++;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            NotePattern pat = new NotePattern(1, E_NoteType.Monster);
            _innerNoteList.Add(pat);

            SpawnNote(5);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NotePattern pat = new NotePattern(2, E_NoteType.Monster);
            _innerNoteList.Add(pat);

            SpawnNote(5);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            NotePattern pat = new NotePattern(3, E_NoteType.Monster);
            _innerNoteList.Add(pat);

            SpawnNote(5);
        }
    }

    private Note GetNoteObject(E_NoteType type)
    {
        switch (type)
        {
            case E_NoteType.None:
                return null;
            case E_NoteType.Monster:
                return ObjPoolManager.Instance.GetObject<Note>(E_Pool.MONSTER_NOTE);
            case E_NoteType.Obstacle:
                return ObjPoolManager.Instance.GetObject<Note>(E_Pool.OBSTACLE_NOTE);
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
