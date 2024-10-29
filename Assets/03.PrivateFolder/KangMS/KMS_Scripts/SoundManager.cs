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
    /// ������������ ������ ������ ȭ�鿡�� �����ϴ� ������ ����մϴ�.
    /// </summary>
    public void PlayLobbyBGM(E_SceneType sceneType)
    {
        _bgmSource.clip = Resources.Load<AudioClip>($"{BGM_PATH}{sceneType.ToString()}");
        _bgmSource.Play();
    }

    /// <summary>
    /// �������� ��ȣ�� �´� ������ ����մϴ�.
    /// </summary>
    public void PlayStageBGM()
    {
        _bgmSource.clip = Resources.Load<AudioClip>
            ($"{BGM_PATH}{DataManager.Instance.StageNumber}");
        _bgmSource.Play();
    }

    /// <summary>
    /// ������ �ߴܽ�ŵ�ϴ�.
    /// </summary>
    public void StopBGM() { _bgmSource.Stop(); }

    /// <summary>
    /// ������ ���������� �����մϴ�.
    /// </summary>
    /// <param name="value">true = fadeIn ���� �ִ� �������� ������ �ø�</param>
    public void FadeBGM(bool value)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        if (value == true)
            _fadeRoutine = StartCoroutine(FadeInBGMVolume());
        else
            _fadeRoutine = StartCoroutine(FadeOutBGMVolume());
    }
     
     public void SetBGMVolume(float value)
     {
        _bgmSource.volume = value;
        DataManager.Instance.SetBGMVolume(value);
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
