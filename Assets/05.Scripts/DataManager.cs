using System.ComponentModel;
using UnityEngine;
public class DataManager : MonoBehaviour, IManager
{
    private static DataManager _instance = null;
    public static DataManager Instance => _instance;
    private DataTable _csvData = new DataTable();
    public DataTable CSVData => _csvData;
    [HideInInspector] public StageData SelectedStageData;
    [HideInInspector] public StageData NextStageData;
    private StageData _stageData = new StageData();
    private GameSettingData _settingData;
    
    public void Init()
    {
        _instance = this;
        
        _settingData.MasterVolume = -20f;
        SetBGMVolume(0.2f); // 유저 정보 저장시 변경
        SetSFXVolume(0.2f);

        LoadPrevData();
    }

    private void Awake()
    {
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, InitDatas, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, ApplySelectStageData, this);
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, CheckStageEndTime, this);
    }

    public void InitDatas() // 스테이지로 전환시 매번 호출되어야함
    {
        _perfectCount = 0;
        _greatCount = 0;
        _isPlaying = false;
        _isStageClear = true;
        _stageProgress = 0;
        _curScore = 0;
        _gameStartTime = Time.time;
    }

    public void LoadPrevData() // 기존 값들 데이터 바인딩
    {
        if (PlayerPrefs.HasKey(SoundManager.MASTER_VOLUME_PLAYERPREFAB))
        {
            _settingData.MasterVolume = PlayerPrefs.GetFloat(SoundManager.MASTER_VOLUME_PLAYERPREFAB);
            SoundManager.Instance.UpdateMasterMixer();
        }
    }

    /// <summary>
    /// 마지막으로 선택된 스테이지 데이터로 적용합니다.
    /// </summary>
    public void ApplySelectStageData()
    {
        SelectedStageData.CopyData(_stageData);
    }

    #region 프로그램 기본 설정
    public int ObjpoolInitCreateCount => 15;
    public float MasterVolume => _settingData.MasterVolume;
    public float BGMVolume => _settingData.BGMVolume;
    public float SFXVolume => _settingData.SFXVolume;

    public void SetBGMVolume(float value) { _settingData.BGMVolume = value; }
    public void SetSFXVolume(float value) { _settingData.SFXVolume = value; }
    #endregion

    #region 스테이지 기초 데이터

    private float _playerMaxHp = 5;
    public float PlayerMaxHP => _playerMaxHp;
    public int SirenCount => 4;
    public float ContactDuration => 4;
    public float ApproachDuration => 0.2f;
    //120bpm이며 * 6 일경우 (0.5f * 6) 음원종료 3 초전에 스폰중단
    public float SkipSpawnTimeOffset => GameManager.Director.BeatInterval * 6;
    
    #endregion

    #region 스테이지 고유 데이터

    public int BPM => _stageData.BPM;
    public float GameSpeed => _stageData.NoteSpeed;
    public float BossHp => _stageData.BossHP;
    public int StageNumber => _stageData.StageNumber;
    public int MeleeCount => _stageData.MeleeCount;
    public string StageName => SelectedStageData.StageName;
    public string SongTitle => SelectedStageData.SongTitle;
    public string StageDescription => SelectedStageData.Description;
    public int NormalPatternLastIdx => 9;
    public int BossPatternLastIdx => 19;
    private BossController _boss;
    public BossController Boss => _boss;
    public Vector3 ContactPos =>
        GameManager.Director.GetCheckPoses(E_SpawnerPosY.MIDDLE);
    public void SetBossData(BossController boss) { this._boss = boss; }
    public void SetPlayerMaxHP(float value) { this._playerMaxHp = value; } // 상점 기능 대응
    
    #endregion

    #region 게임진행 상황 데이터

    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    private bool _isStageClear = true;
    public bool IsStageClear => _isStageClear;
    private float _stageProgress;
    public float StageProgress => _stageProgress; // 0 ~ 1
    private int _greatCount;
    public int GreatCount => _greatCount;
    private int _perfectCount;
    public int PerfectCount => _perfectCount;
    private int _curScore;
    public int CurScore => _curScore;
    private float _gameStartTime;
    private float _gameEndTime;
    public float StagePlayTime => _gameEndTime - _gameStartTime;

    public void CheckStageEndTime() { _gameEndTime = Time.time; }
    public void SetGreatCount(int count) { _greatCount = count; }
    public void SetPerfectCount(int count) { _perfectCount = count; }
    public void SetPlayState(bool value) { _isPlaying = value; } // isclear 와 역할 애매모호
    public void SetStageClear(bool value) { _isStageClear = value; }

    public void AddScore(int value) 
    { 
        _curScore += value;
        UIManager.Instance.SetCurrentScoreText(_curScore.ToString());
    }

    public void UpdatePlayerHP(float value)
    {
        float ratio = value / PlayerMaxHP;
        UIManager.Instance.SetHPValue(ratio);
    }

    public void SetProgress(float current)
    {
        if (SoundManager.Instance.CurrentBgmLength == 0)
        { throw new System.Exception("프로그래스 동기화 순서 문제발생"); }
        _stageProgress = current;
        UIManager.Instance.SetProgressValue(_stageProgress);
        if (_stageProgress >= 1)
            GameManager.Instance.StopProgressTimer();
    }
    #endregion

    #region 씬 관련 수치
    public float SceneFadeDuration => 1f;
    #endregion
    
    #region 음향 관련 설정 수치
    // 볼륨을 서서히 조절하는 비율 ex 0.2f = 현재 음향에서 0.2f비율만큼씩 줄인다.
    public float SoundFadeRate => 0.2f;
    #endregion
}
[System.Serializable]
public class StageData
{
    [Header("스테이지 번호")] public int StageNumber = 1;
    [Space(10f), Header("게임 음원 설정")]
    public E_StageBGM BGM;
    public int BPM;
    public string SongTitle;
    public string StageName;
    public string Description;
    [Range(0.1f, 20f)] public float NoteSpeed = 5f;

    [Space(10f), Header("게임 내 데이터 설정")]
    [SerializeField] private int _meleeCount = 10;
    [SerializeField] private BossStat _bossStat = new BossStat();

    private StageData _nextStage;
    public StageData NextStage => _nextStage;

    public void SetNextStage(StageDataSetter setter)
    {
        _nextStage = setter.StageData;
    }

    public float BossHP => _bossStat.Hp;
    public int BossScore => _bossStat.Score;
    public int MeleeCount => _meleeCount;
    /// <summary>
    /// 현재 개체의 데이터를 target으로 복사합니다.
    /// </summary>
    public void CopyData(StageData target)
    {
        target.StageNumber = this.StageNumber;
        target.BGM = this.BGM;
        target.BPM = this.BPM;
        target.NoteSpeed = this.NoteSpeed;
        target._meleeCount = this._meleeCount;
        target.SongTitle = this.SongTitle;
        target.StageName = this.StageName;
        target.Description = this.Description;
        this._bossStat.CopyData(target._bossStat);
    }
}
public struct GameSettingData
{
    public float BGMVolume;
    public float SFXVolume;
    public float GameSpeed;
    public float MasterVolume;
}