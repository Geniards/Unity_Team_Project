using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Game/AnimationData")]
public class AnimationData : ScriptableObject
{
    public int StageNumber;
    public E_NoteType NoteType;
    public E_NoteSpawnPos NotePosition;
    public E_BossType BossType;  // 보스 타입?
    public RuntimeAnimatorController AnimatorController;
}

