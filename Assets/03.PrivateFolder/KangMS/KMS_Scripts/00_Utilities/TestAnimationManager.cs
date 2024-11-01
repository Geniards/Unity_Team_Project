using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationManager : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController baseAnimatorController;
    [SerializeField] private List<AnimationOverrideData> animationOverrideDataList;

    private void Start()
    {
        GameManager.TestAnimationManager = this;

    }

    public AnimatorOverrideController GetRandomAnimationClip(E_NoteType noteType, E_SpawnerPosY notePosition, int stageNumber)
    {
        if (baseAnimatorController == null)
        {
            Debug.LogError("baseAnimatorController가 할당되지 않았습니다.");
            return null;
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController(baseAnimatorController);

        foreach (var data in animationOverrideDataList)
        {
            Debug.Log($"Checking Data - Stage: {data.stageNumber}, Type: {data.noteType}, Position: {data.notePosition}");

            if (data.stageNumber == stageNumber && data.noteType == noteType && data.notePosition == notePosition)
            {
                AnimationClip randomClip = GetRandomClip(data);
                if (randomClip != null)
                {
                    Debug.Log($"Applying Clip: {randomClip.name} to state Run");
                    overrideController["Run"] = randomClip;
                    return overrideController;
                }
                else
                {
                    Debug.LogWarning("랜덤 클립이 null입니다.");
                }
            }
        }

        Debug.LogWarning("해당 조건에 맞는 애니메이터를 찾을 수 없습니다.");
        return null;
    }

    public AnimationOverrideData GetAnimationData(E_NoteType noteType, E_SpawnerPosY notePosition, int stageNumber)
    {
        foreach (var data in animationOverrideDataList)
        {
            if (data.stageNumber == stageNumber && data.noteType == noteType && data.notePosition == notePosition)
            {
                return data; // 조건에 맞는 데이터를 반환
            }
        }

        Debug.LogWarning("해당 조건에 맞는 애니메이션 데이터를 찾을 수 없습니다.");
        return null;
    }

    /// <summary>
    /// 애니메이션 클립 랜덤 선택
    /// </summary>
    private AnimationClip GetRandomClip(AnimationOverrideData data)
    {
        int randomIndex = Random.Range(0, 3);
        Debug.Log($"Selected Random Index: {randomIndex}");
        switch (randomIndex)
        {
            case 0: return data.clip01;
            case 1: return data.clip02;
            case 2: return data.clip03;
            default: return null;
        }
    }
}
