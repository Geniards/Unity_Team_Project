using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
public class NoteDirector : MonoBehaviour, IManager
{
    private static NoteDirector _instance = null;
    public static NoteDirector Instance => _instance;

    [SerializeField] private NoteSpawner _spawner = null;
    [SerializeField] private NoteSpawnPosController _posController = null;
    private int _bpm;
    private float _noteSpeed;
    private double _noteArriveDuration;
    private float _beatInterval;
    public float BeatInterval => _beatInterval;
    public float TotalHeight =>
        _posController.GetSpawnerPos(E_SpawnerPosY.TOP).y - _posController.GetSpawnerPos(E_SpawnerPosY.BOTTOM).y;
    private Coroutine _spawnRoutine = null;
    private bool _isSkipSpawn = false;
    public Vector3 GetStartSpawnPoses(E_SpawnerPosY posY)
    {
        return _posController.GetSpawnerPos(E_SpawnerPosX.START, posY);
    }
    public Vector3 GetCheckPoses(E_SpawnerPosY posY)
    {
        return _posController.GetSpawnerPos(E_SpawnerPosX.CHECK, posY);
    }
    public Vector3 GetBossPoses(E_SpawnerPosY posY)
    {
        return _posController.GetSpawnerPos(E_SpawnerPosX.BOSS, posY);
    }
    
    private void ChangeBGM()
    {
        StartSpawnNotes(DataManager.Instance.SelectedStageData.BGM+1);
    }
    public void Initailize()
    {
        _noteArriveDuration = CalculateArriveSec();
    }
    /// <summary>
    /// 노트 생성을 시작합니다.
    /// </summary>
    public void StartSpawnNotes(E_StageBGM bgm, int restBeatCount = 0)
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = StartCoroutine(AutoSpawnRoutine(bgm, restBeatCount));
    }

    /// <summary>
    /// 비트 간격의 시간 측정값을 반환합니다.
    /// </summary>
    public float GetBPMtoIntervalSec()
    {
        return 60f / _bpm;
    }
    private double CalculateArriveSec()
    {
        double checkPointDist = _posController.DistSpawnToCheck;
        return Mathf.Abs((float)checkPointDist / _noteSpeed);
    }
    private IEnumerator AutoSpawnRoutine(E_StageBGM bgm, int restBeatCount)
    {
        
        _beatInterval = GetBPMtoIntervalSec();

        double checkedfourBeatSec = _beatInterval * restBeatCount;
        double fourBeatSec = checkedfourBeatSec;

        while (true)
        {
            if (checkedfourBeatSec > _noteArriveDuration
                || restBeatCount == 0)
                break;

            checkedfourBeatSec += fourBeatSec;
        }

        double firstNoteTime
            = AudioSettings.dspTime + checkedfourBeatSec - _noteArriveDuration; // 4박자 뒤의 첫 노트 생성 시간
        
        double nextSpawnTime = firstNoteTime;

        _spawner.RegistPatternData();

        while (true)
        {
            if (DataManager.Instance.IsPlaying == false)
            {
                yield break;
            }
            if (AudioSettings.dspTime >= nextSpawnTime)
            {
                Spawn();
                nextSpawnTime += GetBPMtoIntervalSec();
            }
            yield return null;
        }
    }

    private void Spawn()
    {
        if (_spawner.IsLastNote == true)
        {
            _spawner.RegistPatternData();
        }

        if (_isSkipSpawn == false)
            _spawner.SpawnNote(_noteSpeed);
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

    public void Init()
    {
        _instance = this;

        EventManager.Instance.AddAction(E_Event.SPAWN_STOP, () => { _isSkipSpawn = true; }, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, () => { Note.isBoss = false; }, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, ResetDatas, this);
        EventManager.Instance.AddAction(E_Event.OPENED_STAGESCENE, () => { _isSkipSpawn = false; }, this);
        EventManager.Instance.AddAction(E_Event.SPAWN_START, () => { _isSkipSpawn = false; }, this);
        EventManager.Instance.AddAction
            (E_Event.CHANGED_BGM, ChangeBGM, this);
    }

    private void ResetDatas()
    {
        _bpm = DataManager.Instance.BPM;
        _noteSpeed = DataManager.Instance.GameSpeed;
    }
}