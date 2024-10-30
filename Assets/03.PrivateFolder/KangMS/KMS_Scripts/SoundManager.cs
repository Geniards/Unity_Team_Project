using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    private const string BGM_PATH = "BGMs/";

    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    private Coroutine _fadeRoutine;

    public void Init() 
    { 
        Instance = this;
    }

    /// <summary>
    /// 스테이지씬을 제외한 나머지 화면에서 제공하는 음원을 재생합니다.
    /// </summary>
    public void PlayLobbyBGM(E_SceneType sceneType)
    {
        _bgmSource.clip = Resources.Load<AudioClip>($"{BGM_PATH}{sceneType.ToString()}");
        _bgmSource.Play();
    }

    /// <summary>
    /// 스테이지 번호에 맞는 음원을 재생합니다.
    /// </summary>
    public void PlayStageBGM()
    {
        _bgmSource.clip = Resources.Load<AudioClip>
            ($"{BGM_PATH}{DataManager.Instance.StageNumber}");
        _bgmSource.Play();
        DataManager.Instance.SetBGMClipLength(_bgmSource.clip.length);
    }

    /// <summary>
    /// 음악을 중단시킵니다.
    /// </summary>
    public void StopBGM() { _bgmSource.Stop(); }

    /// <summary>
    /// 서서히 음원볼륨을 조절합니다.
    /// </summary>
    /// <param name="value">true = fadeIn 으로 최대 볼륨까지 서서히 올림</param>
    public void FadeBGM(bool value)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        if (value == true)
            _fadeRoutine = StartCoroutine(FadeInBGMVolume());
        else
            _fadeRoutine = StartCoroutine(FadeOutBGMVolume());
    }
    

    private IEnumerator FadeInBGMVolume()
    {
        float target = DataManager.Instance.BGMVolume;

        while (true)
        {
            if (_bgmSource.volume >= target)
                break;

            _bgmSource.volume =
                Mathf.Lerp(_bgmSource.volume, target, DataManager.Instance.SoundTotalFadeTime * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FadeOutBGMVolume()
    {
        float target = 0;

        while (true)
        {
            while (true)
            {
                if (_bgmSource.volume <= target)
                    break;

                _bgmSource.volume =
                Mathf.Lerp(_bgmSource.volume, target, DataManager.Instance.SoundTotalFadeTime * Time.deltaTime);

                yield return null;
            }
        }
    }

    public void PlaySFX(AudioClip clip) { _sfxSource.PlayOneShot(clip); }
}
