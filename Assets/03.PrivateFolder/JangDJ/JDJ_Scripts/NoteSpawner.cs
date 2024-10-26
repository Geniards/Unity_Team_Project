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

    //// ��� �׽�Ʈ��
    [SerializeField,Space(20f),Header("����׽�Ʈ��")] 
    private Note _notePrefab = null;


    ////

    /// <summary>
    /// �����ʰ� �����ؾ� �� ���� �ʱ�ȭ ��ŵ�ϴ�.
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
    /// ��û���� ���� �ѹ��� �������� �����Ǿ�� �� ��Ʈ �������� �����ʿ� ����صӴϴ�.
    /// </summary>
    //public void RegistPattern(int patternNumber) // ��� ��������
    //{
    // �ش� CSV �� ���� �ش� ������ ������ �ҷ��´�.
    // NotePattern ����Ʈ ����� �ҷ��ͼ� ��ü�� ���� ����

    //_innerNoteList.Clear(); //  �Ź� ���� �δ㽺����, �ε����� ����Ѵ�.

    //    List<NotePattern> patterns = CSVLoader.GetPattern(patternNumber);

    //    for (int i = 0; i < patterns.Count; i++)
    //    {
    //        _innerNoteList.Add(patterns[i]);
    //    }
    //}

    /// <summary>
    /// ��� �Ǿ��ִ� ��Ʈ ���� ��Ͽ�Ҹ� ��üȭ�մϴ�.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        if (_lastNoteIdx >= _innerNoteList.Count)
        { throw new System.Exception("��ϵ� ��Ʈ�� �����ϴ�."); }

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
