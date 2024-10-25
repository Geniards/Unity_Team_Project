using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    public void Init() 
    { 
        Instance = this; 
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
