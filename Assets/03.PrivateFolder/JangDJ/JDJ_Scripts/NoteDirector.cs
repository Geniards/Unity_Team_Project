using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class NoteDirector : MonoBehaviour
{
    [SerializeField] private NoteSpawner _spawner = null;
    [SerializeField] private NoteSpawnPosController _posController = null;

    private int _bpm;
    private float _noteSpeed;
    private double _noteArriveDuration;
    [SerializeField] private float _prevDelay;

    private Coroutine _spawnRoutine = null;

    private bool _isSkipSpawn = false;

    /// <summary>
    /// 다음 박자부터 스폰을 중단합니다.
    /// </summary>
    public void SetSpawnSkip(bool isSkip)
    {
        _isSkipSpawn = isSkip;
    }

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

    private void Awake()
    {
        if (GameManager.NoteDirector != null)
            Destroy(GameManager.NoteDirector);

        GameManager.NoteDirector = this;
        _bpm = DataManager.Instance.BPM;
        _noteSpeed = DataManager.Instance.GameSpeed;
    }

    public void Initailize()
    {
        _noteArriveDuration = CalculateArriveSec();
    }

    /// <summary>
    /// 노트 생성을 시작합니다.
    /// </summary>
    public void StartSpawnNotes()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = StartCoroutine(AutoSpawnRoutine());
    }

    private float GetBPMtoIntervalSec()
    {
        return 60f / _bpm;
    }

    private double CalculateArriveSec()
    {
        double checkPointDist = _posController.DistSpawnToCheck;
        return Mathf.Abs((float)checkPointDist / _noteSpeed);
    }

    private IEnumerator AutoSpawnRoutine()
    {
        double nextSpawnTime = 0d;
        double startDspTime = AudioSettings.dspTime;
        double firstNoteTime = startDspTime + (GetBPMtoIntervalSec() * 4) - _noteArriveDuration; // 4박자 뒤의 첫 노트 생성 시간
                                                                                                 // 첫 번째 노트 생성 타이밍 설정
        nextSpawnTime = firstNoteTime;
        _spawner.RegistPattern(1); // 임시 테스트 코드
        SoundManager.Instance.PlayStageBGM();

        nextSpawnTime = AudioSettings.dspTime + GetBPMtoIntervalSec();

        while (true)
        {
            if (DataManager.Instance.IsPlaying == false)
            {
                yield break;
            }

            if(AudioSettings.dspTime >= nextSpawnTime)
            {
                if (_spawner.IsLastNote == true)
                {
                    _spawner.RegistPattern(1); // 임시 테스트 코드
                }

                if(_isSkipSpawn == false)
                    _spawner.SpawnNote(_noteSpeed);
                
                //_posController.NoteCheckRay();

                nextSpawnTime += GetBPMtoIntervalSec();
            }
            
            yield return null;
        }
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

}