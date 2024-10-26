using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    private Dictionary<int, List<NotePattern>> _patternTable = null;

    /// <summary>
    /// ������ ���̺��� �ʱ�ȭ�� �����մϴ�.
    /// </summary>
    public void Initailize()
    {
        _patternTable = new Dictionary<int, List<NotePattern>>();
    }

    /// <summary>
    /// ������ ���̺� ���� ������ ����մϴ�.
    /// </summary>
    public void RegistPattern(int stageNumber, NotePattern pattern)
    {
        if(!_patternTable.ContainsKey(stageNumber))
        { _patternTable.Add(stageNumber, new List<NotePattern>()); }

        _patternTable[stageNumber].Add(pattern);
    }

    //public List<NotePattern> GetPattern(int patternNumber)
    //{
    //    return _patternTable[GameManager.Instance.StageNumber]
    //}
}
