using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    public void PlayBGM(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM() { _bgmSource.Stop(); }

    public void PlaySFX(AudioClip clip) { _sfxSource.PlayOneShot(clip); }

    public void SetBGMVolume(float volume) { _bgmSource.volume = volume; }

    public void SetSFXVolume(float volume) { _sfxSource.volume = volume; }
}
