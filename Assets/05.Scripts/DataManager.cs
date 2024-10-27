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
    }

    public int ObjpoolInitCreateCount => 5;

    public void SetPlayerHP(int value) { _stageData.PlayerHp = value; }
    public void AddPlayerHP(int value) { _stageData.PlayerHp += value; }
    public void SetJudge(E_NoteDecision type) { _stageData.Judge = type; }
    public void SetProgress(float value) { _stageData.StageProgress = value; }
    public void SetComboCount(int value) { _stageData.ComboCount = value; }

    public void SetBGMVolume(float value) { _settingData.BGMVolume = value; }
    public void SetSFXVolume(float value) { _settingData.SFXVolume = value; }
    public void SetGameSpeed(float value) { _settingData.GameSpeed = value; }
}

public struct StageData
{
    public int PlayerHp;
    public int BossHp;
    public E_NoteDecision Judge;
    public float StageProgress;
    public int Score;
    public int ComboCount;
}

public struct GameSettingData
{
    public float BGMVolume;
    public float SFXVolume;
    public float GameSpeed;
}




