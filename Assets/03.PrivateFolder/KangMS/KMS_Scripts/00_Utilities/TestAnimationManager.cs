using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationManager : MonoBehaviour
{
    private static TestAnimationManager _instance = null;
    public static TestAnimationManager Instance => _instance;

    // 애니메이터 매핑 데이터 리스트
    [SerializeField] private List<AnimationData> animationDataList;

    public void Init()
    {
        _instance = this;
    }

    /// <summary>
    /// 노트 타입, 위치, 스테이지 번호에 맞는 애니메이터를 반환
    /// </summary>
    public RuntimeAnimatorController GetAnimatorController(E_NoteType noteType, E_NoteSpawnPos notePosition, int stageNumber)
    {
        foreach (var data in animationDataList)
        {
            if (data.StageNumber == stageNumber && data.NoteType == noteType && data.NotePosition == notePosition)
            {
                return data.AnimatorController;
            }
        }

        Debug.LogWarning("해당 조건에 맞는 애니메이터를 찾을 수 없습니다.");
        return null;
    }
}
