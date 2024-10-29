using System.ComponentModel;
using UnityEngine;

public class DataManager : MonoBehaviour, IManager
{
    private static DataManager _instance = null;
    public static DataManager Instance => _instance;

    private DataTable _csvData = null;
    public DataTable CSVData => _csvData;

    private StageData _stageData;
    private GameSettingData _settingData;

    public void Init()
    {
        _instance = this;
        _csvData = new DataTable();
        _csvData.Initailize();

        SetBGMVolume(1); // 유저 정보 저장시 변경
        SetSFXVolume(1);
        SetStageNumber(1);
    }

    [SerializeField, Header("분당 Beat")]
    private int _bpm = 120;
    public int BPM => _bpm;

    [SerializeField,Range(1,20),Header("전체 게임 속도")] 
    private int _gameSpeed = 1;
    public int GameSpeed => _gameSpeed;

    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    // 볼륨을 서서히 조절하는 비율
    public float SoundFadeRate => 0.2f;
    // 한계값에 도달하는 시간
    public float SoundTotalFadeTime => 1f;
    // 현재 재생되고 있는 음원의 총 길이
    public float CurrentBGMClipLength => _stageData.CurrentBGMClipLength;

    public int StageNumber => _stageData.StageNumber;

    public int ObjpoolInitCreateCount => 5;
    public float BGMVolume => _settingData.BGMVolume;
    public float SFXVolume => _settingData.SFXVolume;

    public float PlayerHp => _stageData.PlayerHp;
    public float StageProgress => _stageData.StageProgress;

    public void SetPlayState(bool value) { _isPlaying = value; }
    public void SetBGMClipLength(float value) { _stageData.CurrentBGMClipLength = value; }

    public void SetPlayerHP(float value) { _stageData.PlayerHp = value; }
    public void SetBossHP(float value) { _stageData.BossHp = value; }
    public void SetJudge(E_NoteDecision type) { _stageData.Judge = type; }
    public void SetProgress(float value) { _stageData.StageProgress = value; }
    public void SetComboCount(int value) { _stageData.ComboCount = value; }
    public void SetStageNumber(int value) { _stageData.StageNumber = value; }

    public void SetBGMVolume(float value) { _settingData.BGMVolume = value; }
    public void SetSFXVolume(float value) { _settingData.SFXVolume = value; }

    
}

public struct StageData
{
    public float PlayerHp;
    public float BossHp;
    public E_NoteDecision Judge;
    public float StageProgress;
    public int Score;
    public int ComboCount;
    public int StageNumber;
    public float CurrentBGMClipLength;
}

public struct GameSettingData
{
    public float BGMVolume;
    public float SFXVolume;
    public float GameSpeed;
}





