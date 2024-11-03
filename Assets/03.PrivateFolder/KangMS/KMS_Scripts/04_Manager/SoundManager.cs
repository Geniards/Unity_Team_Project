using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour, IManager
{
    #region BIND KEY

    public const string MASTER_VOLUME_PLAYERPREFAB= "MasterVolume";
    public const string MASTER_VOLUME_MIXER_KEY = "MainSound";

    #endregion

    private const string BGM_STAGE_PATH = "BGMs/Stage/";
    private const string BGM_MAIN_PATH = "BGMs/Main/";

    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private SFXList _sfxList;
    private E_StageBGM _currentStageBgm = E_StageBGM.NONE;
    public float CurrentBgmLength => _bgmSource.clip.length;

    private Coroutine _fadeRoutine;

    [SerializeField] private AudioMixer _mixer;

    private Slider _soundSlider;
    public Slider SoundSlider
    {
        get => _soundSlider;
        set
        {
            _soundSlider = value;

            _soundSlider.onValueChanged.AddListener(SoundControl);
        }
    }

    public void UpdateMasterMixer()
    {
        _mixer.SetFloat(MASTER_VOLUME_MIXER_KEY, DataManager.Instance.MasterVolume);
    }

    private void SoundControl(float volume)
    {
        _mixer.SetFloat(MASTER_VOLUME_MIXER_KEY, volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_PLAYERPREFAB, volume);
    }

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
    /// 선택한 타입의 음원이 재생됩니다.
    /// </summary>
    public void PlayBGM(E_StageBGM bgmType)
    {
        _bgmSource.loop = false;

        _bgmSource.clip = Resources.Load<AudioClip>
            ($"{BGM_STAGE_PATH}{bgmType}");
        _bgmSource.Play();
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
    public void FadeBGM(bool value,float duration = 1, float targetVolume = float.MaxValue)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        float volume = value == true ? DataManager.Instance.BGMVolume : 0;

        if(targetVolume != float.MaxValue)
        {
            volume = targetVolume;
        }


        if (value == true)
        {
            _fadeRoutine = StartCoroutine(FadeInBGMVolume(duration, volume));
        }
            
        else
        {
            _fadeRoutine = StartCoroutine(FadeOutBGMVolume(duration, volume));
        }
            
    }
     
     public void SetBGMVolume(float value)
     {
        if (_bgmSource == null)
        {
            Debug.LogWarning("BGM 소스가 존재하지 않습니다.");
            return;
        }

        _bgmSource.volume = value;
        DataManager.Instance.SetBGMVolume(value);
     }

    private IEnumerator FadeInBGMVolume(float duration,float targetVolume)
    {
        float timer = 0;
        float amount = targetVolume / duration;

        while (true)
        {
            if (_bgmSource.volume >= targetVolume)
                break;

            _bgmSource.volume += amount * Time.deltaTime;

            yield return null;
        }

        _bgmSource.volume = targetVolume;
    }

    private IEnumerator FadeOutBGMVolume(float duration, float targetVolume)
    {
        float amount = _bgmSource.volume / duration;

        while (true)
        {
            if (_bgmSource.volume <= targetVolume)
                break;

            _bgmSource.volume -= amount * Time.deltaTime;
            yield return null;
        }

        _bgmSource.volume = targetVolume;
    }

    private IEnumerator SirenRoutine(float length)
    {
        float timer = length;
        int count = 1;

        PlaySFX(E_SFX.SIREN);

        while (true)
        {
            if (count >= DataManager.Instance.SirenCount)
                break;
            
            if(timer <= 0)
            {
                timer = length;
                count++;
                PlaySFX(E_SFX.SIREN);

                if (count == DataManager.Instance.SirenCount - 1)
                    GameManager.Instance.CreateBoss();
            }

            timer -= Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator PlayBossBGMRoutine()
    {
        float oneLoopLength = _sfxList[E_SFX.SIREN].length;

        yield return SirenRoutine(oneLoopLength);

        FadeBGM(true, 2.5f);

        PlayBGM(_currentStageBgm + 1);
        _bgmSource.loop = true;
    }

    public void PlayBossBGM()
    {
        EventManager.Instance.PlayEvent(E_Event.CHANGED_BGM);
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
