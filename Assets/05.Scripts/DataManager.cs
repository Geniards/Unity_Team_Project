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

        SetBGMVolume(1); // ���� ���� ����� ����
        SetSFXVolume(1);
        SetStageNumber(1);
    }

    [SerializeField, Header("�д� Beat")]
    private int _bpm = 120;
    public int BPM => _bpm;

    [SerializeField,Range(1,20),Header("��ü ���� �ӵ�")] 
    private int _gameSpeed = 1;
    public int GameSpeed => _gameSpeed;

    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    // ������ ������ �����ϴ� ����
    public float SoundFadeRate => 0.2f;
    // �Ѱ谪�� �����ϴ� �ð�
    public float SoundTotalFadeTime => 1f;

    public int StageNumber => _stageData.StageNumber;

    public int ObjpoolInitCreateCount => 5;
    public float BGMVolume => _settingData.BGMVolume;
    public float SFXVolume => _settingData.SFXVolume;

    public void SetPlayState(bool value) { _isPlaying = value; }

    public void SetPlayerHP(int value) { _stageData.PlayerHp = value; }
    public void AddPlayerHP(int value) { _stageData.PlayerHp += value; }
    public void SetJudge(E_NoteDecision type) { _stageData.Judge = type; }
    public void SetProgress(float value) { _stageData.StageProgress = value; }
    public void SetComboCount(int value) { _stageData.ComboCount = value; }
    public void SetStageNumber(int value) { _stageData.StageNumber = value; }

    public void SetBGMVolume(float value) { _settingData.BGMVolume = value; }
    public void SetSFXVolume(float value) { _settingData.SFXVolume = value; }
}

public struct StageData
{
    public int PlayerHp;
    public int BossHp;
    public E_NoteDecision Judge;
    public float StageProgress;
    public int Score;
    public int ComboCount;
    public int StageNumber;
}

public struct GameSettingData
{
    public float BGMVolume;
    public float SFXVolume;
    public float GameSpeed;
}




