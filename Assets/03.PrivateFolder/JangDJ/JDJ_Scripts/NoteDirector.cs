using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class NoteDirector : MonoBehaviour
{
    [SerializeField] private NoteSpawner _spawner = null;
    [SerializeField] private NoteSpawnPosController _posController = null;

    [SerializeField] private int _bpm;
    [SerializeField] private float _noteSpeed;
    [ReadOnly(true)] private float _noteArriveDuration;
    [SerializeField] private float _prevDelay;

    private Coroutine _spawnRoutine = null;
    private WaitForSeconds _intervalSec = null;

    public int BPM => _bpm;

    public Vector3 GetCheckPoses(E_SpawnerPosY posY)
    {
        return _posController.GetSpawnerPos(E_SpawnerPosX.CHECK, posY);
    }

    private void Awake()
    {
        if (GameManager.NoteDirector != null)
            Destroy(GameManager.NoteDirector);

        GameManager.NoteDirector = this;
    }

    /// <summary>
    /// �����ϴ� BPM �� ��Ʈ �̵��ӵ��� ���� ���� ���� ������ �ʱ�ȭ����
    /// </summary>
    public void Initailize()
    {
        //_bpm = bpm;
        //_noteSpeed = speed;

        _intervalSec = new WaitForSeconds(GetBPMtoIntervalSec());
        _noteArriveDuration = CalculateArriveSec();
    }

    /// <summary>
    /// �������� ����� ����Ǵ� ���� �ݺ������� ��Ʈ�� �����մϴ�.
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

    private float CalculateArriveSec()
    {
        float checkPointDist = _posController.DistSpawnToCheck;
        return Mathf.Abs(checkPointDist / _noteSpeed);
    }

    private IEnumerator AutoSpawnRoutine()
    {
        _spawner.RegistPattern(1); // �ӽ��� 1

        yield return new WaitForSeconds(_prevDelay + _noteArriveDuration);

        while (true)
        {
            // �߰� ������ ��� �ߴ��ϴ� ��� ������
            if (GameManager.Instance.IsPlaying == false)
            {
                yield break;
            }

            if (_spawner.IsLastNote == true)
            {
                _spawner.RegistPattern(1); // �ӽ��� 1
            }

            _spawner.SpawnNote(_noteSpeed);

            yield return _intervalSec;
        }
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }

}
