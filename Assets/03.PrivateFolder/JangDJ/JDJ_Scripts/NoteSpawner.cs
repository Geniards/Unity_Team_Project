using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private NoteSpawnPosController _posController = null;
    private List<NotePattern> _innerNoteList = null;
    private int _lastNoteIdx = 0;

    public bool IsLastNote
        => _lastNoteIdx == _innerNoteList.Count - 1;

    /// <summary>
    /// 스포너가 설정해야 할 값을 초기화 시킵니다.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        _innerNoteList = new List<NotePattern>();
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
        Note note = null;

        switch (pattern.noteType)
        {
            case E_NoteType.None:
                return;

            case E_NoteType.Monster:
                note = ObjPoolManager.Instance.GetObject<Note>(E_Pool.MONSTER);
                break;

            case E_NoteType.Obstacle:
                note = ObjPoolManager.Instance.GetObject<Note>(E_Pool.OBSTACLE);
                break;

                // 보스...?
        }

        switch (pattern.position)
        {
            case 1:
                note.transform.position = _posController.GetSpawnerPos(E_SpawnerPosY.TOP);
                break;
            case 2:
                note.transform.position = _posController.GetSpawnerPos(E_SpawnerPosY.MIDDLE);
                break;
            case 3:
                note.transform.position = _posController.GetSpawnerPos(E_SpawnerPosY.BOTTOM);
                break;
        }

        //note.SetSpeed(noteSpeed);
    }
}
