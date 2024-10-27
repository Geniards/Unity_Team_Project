using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    private enum E_SpawnTRs
    {
        NONE = -1,
        SPAWN,
        CHECKPOINT,
        END,
        E_SPAWNTRS_MAX
    }

    //private List<NotePattern> _innerNoteList = null;
    private int _lastNoteIdx = 0;

    // �� TR ����ȭ �ʿ�
    // ��� ������
    [SerializeField] private List<Transform> _topTrs;
    // �߰� ������
    [SerializeField] private List<Transform> _midTrs;
    // �ϴ� ������
    [SerializeField] private List<Transform> _botTrs;

    public float DistSpawnToCheck
        => Vector3.Distance(_topTrs[(int)E_SpawnTRs.SPAWN].position, _topTrs[(int)E_SpawnTRs.END].position);

    //public bool IsLastNote
    //    => _lastNoteIdx == _innerNoteList.Count - 1;

    /// <summary>
    /// �����ʰ� �����ؾ� �� ���� �ʱ�ȭ ��ŵ�ϴ�.
    /// </summary>
    public void Initalize()
    {
        _lastNoteIdx = 0;

        //_innerNoteList = List<NotePattern>();
    }

    /// <summary>
    /// ��û���� ���� �ѹ��� �������� �����Ǿ�� �� ��Ʈ �������� �����ʿ� ����صӴϴ�.
    /// </summary>
    public void RegistPattern(int patternNumber) // ��� ��������
    {
        // �ش� CSV �� ���� �ش� ������ ������ �ҷ��´�.
        // NotePattern ����Ʈ ����� �ҷ��ͼ� ��ü�� ���� ����

        //_innerNoteList.Clear(); //  �Ź� ���� �δ㽺����, �ε����� ����Ѵ�.
        
        //List<NotePattern> patterns = CSV.GetPattern(patternNumber);

        //for (int i = 0; i < patterns.Count; i++)
        //{
        //    _innerNoteList.Add(patterns[i]);
        //}
    }

    /// <summary>
    /// ��� �Ǿ��ִ� ��Ʈ ���� ��Ͽ�Ҹ� ��üȭ�մϴ�.
    /// </summary>
    public void SpawnNote(float noteSpeed)
    {
        //if(_lastNoteIdx >= _innerNoteList.Count)
        //{ throw new System.Exception("��ϵ� ��Ʈ�� �����ϴ�."); }

        //PatternNote pattern = _innerNoteList[_lastNoteIdx];
        //Note note = null;

        //switch (pattern.NoteType)
        //{
        //    case None:
        //        return;

        //    case Monster:
        //        note = ObjPoolManager.Instance.GetObject<Note>(E_Pool.MONSTER);
        //        break;

        //    case Obstacle:
        //        note = ObjPoolManager.Instance.GetObject<Note>(E_Pool.OBSTACLE);
        //        break;

        //        // ����...?
        //}

        //switch (pattern.position)
        //{
        //    case 1:
        //        note.tr.postion = _topTrs[(int)E_SpawnTRs.SPAWN].position;
        //        break;
        //    case 2:
        //        note.tr.postion = _midTrs[(int)E_SpawnTRs.SPAWN].position;
        //        break;
        //    case 3:
        //        note.tr.postion = _botTrs[(int)E_SpawnTRs.SPAWN].position;
        //        break;
        //}

        //note.SetSpeed(noteSpeed);
    }
}
