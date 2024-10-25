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
    /// �����ʰ� �����ؾ� �� ���� �ʱ�ȭ ��ŵ�ϴ�.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        _innerNoteList = new List<NotePattern>();
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

                // ����...?
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
