using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAnimOverrideData", menuName = "Game/BossAnimationOverrideData")]
public class BossAnimOverrideData : ScriptableObject
{
    public int stageNumber;
    public E_NoteType noteType;
    public E_SpawnerPosY notePosition;
    public List<AnimationClip> playAnimClip;
}
