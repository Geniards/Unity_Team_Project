using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXList
{
    [SerializeField] private List<SFXItem> _sfxList;

    public AudioClip this[E_SFX type]
    {
        get { return _sfxList[(int)type].SFX; }
    }

    public float GetLength(E_SFX type)
    {
        return _sfxList[(int)type].Length;
    }
}

[System.Serializable]
public class SFXItem
{
    [SerializeField] private E_SFX _sfxType;
    [SerializeField] private AudioClip _sfx;

    public AudioClip SFX => _sfx;
    public float Length => _sfx.length;
}