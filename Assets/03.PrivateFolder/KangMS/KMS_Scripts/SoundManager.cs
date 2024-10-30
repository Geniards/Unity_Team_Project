using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    private const string BGM_STAGE_PATH = "BGMs/Stage/";
    private const string BGM_MAIN_PATH = "BGMs/Main/";

    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private SFXList _sfxList;
    private E_StageBGM _currentStageBgm = E_StageBGM.NONE;

    private Coroutine _fadeRoutine;

    public void Init() 
    { 
        Instance = this;
        _currentStageBgm = E_StageBGM.NONE;
    }

    /// <summary>
    /// 스테이지씬을 제외한 나머지 화면에서 제공하는 음원을 재생합니다.
    /// </summary>
    public void PlayLobbyBGM(E_SceneType sceneType)
    {
        _bgmSource.clip = Resources.Load<AudioClip>($"{BGM_STAGE_PATH}{sceneType.ToString()}");
        _bgmSource.Play();
    }

    /// <summary>
    /// 스테이지 번호에 맞는 음원을 재생합니다.
    /// </summary>
    public void PlayBGM(E_StageBGM bgmType)
    {
        _bgmSource.loop = false;

        _bgmSource.clip = Resources.Load<AudioClip>
            ($"{BGM_STAGE_PATH}{bgmType}");
        _bgmSource.Play();
        DataManager.Instance.SetBGMClipLength(_bgmSource.clip.length);
        _currentStageBgm = bgmType;
    }

    public void PlayBGM(E_MainBGM bgmType)
    {
        _bgmSource.clip = Resources.Load<AudioClip>
            ($"{BGM_STAGE_PATH}{bgmType}");
        _bgmSource.Play();
    }

    /// <summary>
    /// 음악을 중단시킵니다.
    /// </summary>
    public void StopBGM() { _bgmSource.Stop(); }

    /// <summary>
    /// 서서히 음원볼륨을 조절합니다.
    /// </summary>
    /// <param name="value">true = fadeIn 으로 최대 볼륨까지 서서히 올림</param>
    public void FadeBGM(bool value,float duration = 1)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        if (value == true)
            _fadeRoutine = StartCoroutine(FadeInBGMVolume(duration));
        else
            _fadeRoutine = StartCoroutine(FadeOutBGMVolume(duration));
    }
     
     public void SetBGMVolume(float value)
     {
        _bgmSource.volume = value;
        DataManager.Instance.SetBGMVolume(value);
     }

    private IEnumerator FadeInBGMVolume(float duration)
    {
        float target = DataManager.Instance.BGMVolume;
        float timer = 0;
        float amount = target / duration;

        while (true)
        {
            if (_bgmSource.volume >= target)
                break;

            _bgmSource.volume += amount * Time.deltaTime;

            yield return null;
        }

        _bgmSource.volume = target;
    }

    private IEnumerator FadeOutBGMVolume(float duration)
    {
        float target = 0;
        float timer = 0;
        float amount = _bgmSource.volume / duration;

        while (true)
        {
            if (_bgmSource.volume <= target)
                break;

            _bgmSource.volume -= amount * Time.deltaTime;
            yield return null;
        }

        _bgmSource.volume = target;
    }

    private IEnumerator SirenRoutine(float length)
    {
        float timer = length;
        int count = 0;

        PlaySFX(E_SFX.SIREN);

        while (true)
        {
            if (count >= DataManager.Instance.SirenCount)
                break;
            else
                

            if(timer <= 0)
            {
                timer = length;
                count++;
                PlaySFX(E_SFX.SIREN);
            }

            timer -= Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator PlayBossBGMRoutine()
    {
        float oneLoopLength = _sfxList[E_SFX.SIREN].length;

        yield return SirenRoutine(oneLoopLength);

        FadeBGM(true, 3f);

        GameManager.NoteDirector.SetSpawnSkip(false);
        PlayBGM(_currentStageBgm + 1);
        _bgmSource.loop = true;
    }

    public void PlayBossBGM()
    {
        StartCoroutine(PlayBossBGMRoutine());
    }

    /// <summary>
    /// SFX 타입의 음원을 재생합니다.
    /// </summary>
    public void PlaySFX(E_SFX sfxType) 
    {
        _sfxSource.PlayOneShot(_sfxList[sfxType]);
    }
}
