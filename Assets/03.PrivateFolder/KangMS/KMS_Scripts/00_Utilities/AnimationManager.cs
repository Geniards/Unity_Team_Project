using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("기본 애니메이터 컨트롤러")]
    [SerializeField] private RuntimeAnimatorController baseAnimatorController;
    [Header("노트 애니메이션 데이터")]
    [SerializeField] private List<AnimationOverrideData> animationOverrideDataList;

    private void Start()
    {
        GameManager.AnimationChanger = this;
    }

    /// <summary>
    /// Note 타입, 위치, 스테이지 번호에 따라 개별적인 AnimatorOverrideController를 생성하여 반환
    /// </summary>
    public AnimatorOverrideController GetRandomAnimationController(E_NoteType noteType, E_SpawnerPosY notePosition, int stageNumber)
    {
        if (!baseAnimatorController)
        {
            Debug.LogError("baseAnimatorController가 할당되지 않았습니다.");
            return null;
        }

        // 개별 Note에 적용될 AnimatorOverrideController를 새로 생성
        AnimatorOverrideController overrideController = new AnimatorOverrideController(baseAnimatorController);

        foreach (var data in animationOverrideDataList)
        {
            if (data.stageNumber == stageNumber && data.noteType == noteType && data.notePosition == notePosition)
            {
                AnimationClip randomClip = GetRandomClip(data);
                if (randomClip != null)
                {
                    overrideController["Run"] = randomClip;
                    return overrideController;
                }
                else
                {
                    Debug.LogWarning("랜덤 클립이 null입니다.");
                }
            }
        }

        Debug.LogWarning("해당 조건에 맞는 애니메이션 데이터를 찾을 수 없습니다.");
        return overrideController;
    }

    /// <summary>
    /// 보스 애니메이션에 대한 개별 AnimatorOverrideController 생성 및 반환
    /// </summary>
    public AnimatorOverrideController GetAnimationController(int stageNumber, string bossState, int clipIndex)
    {
        if (!baseAnimatorController)
        {
            Debug.LogError("baseAnimatorController가 할당되지 않았습니다.");
            return null;
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController(baseAnimatorController);

        foreach (var data in animationOverrideDataList)
        {
            if (data.stageNumber == stageNumber /*&& data.bossState == bossState*/)
            {
                AnimationClip selectedClip = GetAnimationClip(data, clipIndex);
                if (selectedClip != null)
                {
                    // 사용할 기본 애니메이션 클립 이름이 "Run"이라고 가정
                    overrideController["/*변경할 기본 클립 이름*/"] = selectedClip;
                    return overrideController;
                }
                else
                {
                    Debug.LogWarning("애니메이션 클립이 null입니다.");
                }
            }
        }

        Debug.LogWarning("해당 조건에 맞는 애니메이션 데이터를 찾을 수 없습니다.");
        return overrideController;
    }

    /// <summary>
    /// 애니메이션 클립 랜덤 선택
    /// </summary>
    private AnimationClip GetRandomClip(AnimationOverrideData data)
    {
        int randomIndex = Random.Range(0, data.playAnimClip.Count);
        return data.playAnimClip[randomIndex];
    }

    /// <summary>
    /// 보스 애니메이션 클립 선택
    /// </summary>
    private AnimationClip GetAnimationClip(AnimationOverrideData data, int clipIndex)
    {
        // 보스는 상태에 따른 하나의 애니메이션 클립을 갖는다고 가정
        return data.playAnimClip[clipIndex];
    }
}
