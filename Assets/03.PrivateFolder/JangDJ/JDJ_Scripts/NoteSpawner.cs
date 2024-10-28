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
        => _lastNoteIdx == _innerNoteList.Count - 1;

    /// <summary>
    /// �����ʰ� �����ؾ� �� ���� �ʱ�ȭ ��ŵ�ϴ�.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        _innerNoteList = new List<NoteData>();
    }

    private void Start()
    {
        Initalize();
    }

    /// <summary>
    /// ��û���� ���� �ѹ��� �������� �����Ǿ�� �� ��Ʈ �������� �����ʿ� ����صӴϴ�.
    /// </summary>
    public void RegistPattern(int patternNumber) // ��� ��������
    {
        //List<NotePattern> patterns = CSVLoader.GetPattern(patternNumber);

        //for (int i = 0; i < patterns.Count; i++)
        //{
        //    _innerNoteList.Add(patterns[i]);
        //}

        _innerNoteList.Add(new NoteData(1, E_NoteType.Score));
        _innerNoteList.Add(new NoteData(2, E_NoteType.Score));
        _innerNoteList.Add(new NoteData(3, E_NoteType.Score));
        _innerNoteList.Add(new NoteData(1, E_NoteType.Score));
    }

    /// <summary>
    /// ��� �Ǿ��ִ� ��Ʈ ���� ��Ͽ�Ҹ� ��üȭ�մϴ�.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        if (_lastNoteIdx >= _innerNoteList.Count)
        { throw new System.Exception("��ϵ� ��Ʈ�� �����ϴ�."); }

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
        }

        throw new System.Exception("�߸��� ��Ʈ ��û");
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
        throw new System.Exception("�߸��� ��Ʈ ��ġ ��û");
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

        throw new System.Exception("�߸��� ��Ʈ ��ġ ��û");
    }


    
}
