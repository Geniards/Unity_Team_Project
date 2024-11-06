using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAnimOverrideData", menuName = "Game/BossAnimationOverrideData")]
public class BossAnimOverrideData : ScriptableObject
{
    public int stageNumber;
    // 사용할 보스 Enum문이거나 상태
    //
    public List<AnimationClip> playAnimClip;
}
