using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationOverrideData", menuName = "Game/AnimationOverrideData")]
public class AnimationOverrideData : ScriptableObject
{
    public int stageNumber;
    public E_NoteType noteType;
    public E_SpawnerPosY notePosition;
    public List<AnimationClip> playAnimClip;
}
