using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    private Dictionary<int, List<NotePattern>> _patternTable =
        new Dictionary<int, List<NotePattern>>();

    /// <summary>
    /// 데이터 테이블의 초기화를 진행합니다.
    /// </summary>
    public void Initailize()
    {
    }

    /// <summary>
    /// 데이터 테이블에 패턴 정보를 등록합니다.
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
