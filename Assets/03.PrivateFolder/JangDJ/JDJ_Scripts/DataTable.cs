using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    private Dictionary<int, List<NotePattern>> _patternTable =
        new Dictionary<int, List<NotePattern>>();

    /// <summary>
    /// 현재 스테이지에 맞는 노트 묶음 반환
    /// </summary>
    public List<NoteData> this[int idx]
    {
        get 
        {
            int _curStage = DataManager.Instance.StageNumber;

            return _patternTable[_curStage][idx].Notes;
        }
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
