using System.Collections.Generic;
using UnityEngine;

public class NoteSpawnPosController : MonoBehaviour
{
    [Header("스포너의 기준 X 값을 측정하기 위한 트랜스폼")]
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _checkPos;
    [SerializeField] private Transform _endPos;

    [Space(20f), Header("스포너의 기준 Y 값을 측정하기 위한 트랜스폼")] 
    [SerializeField] private Transform _bottomPointPos;
    [SerializeField] private Transform _bottomToIntervalSpawners;

    private List<float> _posesXvalues = null; // end , check , start
    private List<float> _posesYvalues = null; // bot , mid , top

    // 스폰 시작지점으로 부터 체크포인트까지의 거리값
    public float DistSpawnToCheck =>
        _posesXvalues[(int)E_SpawnerPosX.CHECK] - _posesXvalues[(int)E_SpawnerPosX.START];

    private void Start()
    {
        _posesXvalues = new List<float>();
        _posesYvalues = new List<float>();

        RegistXValues();
        RegistYValues();
    }

    private void RegistXValues()
    {
        _posesXvalues.Add(_startPos.position.x);
        _posesXvalues.Add(_checkPos.position.x);
        _posesXvalues.Add(_endPos.position.x);
    }

    private void RegistYValues()
    {
        float interval =
            _bottomToIntervalSpawners.position.y - _bottomPointPos.position.y;

        _posesYvalues.Add(_bottomPointPos.position.y);
        _posesYvalues.Add(_bottomToIntervalSpawners.position.y);
        _posesYvalues.Add(_bottomToIntervalSpawners.position.y + interval);
    }

    private void Update()
    {
        for (int i = 0; i < _posesYvalues.Count; i++)
        {
            DrawRay(new Vector3(_startPos.position.x, _posesYvalues[i]),
                Vector3.left, Color.green);
        }

        for (int i = 0; i < _posesXvalues.Count; i++)
        {
            DrawRay(new Vector3(_posesXvalues[i], _posesYvalues[_posesYvalues.Count-1]),
                Vector3.down, Color.red);
        }
    }

    /// <summary>
    /// 원하는 각 지점의 위치값을 반환 받습니다.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosX posX, E_SpawnerPosY posY)
    {
        return new Vector3(_posesXvalues[(int)posX], _posesYvalues[(int)posY]);
    }

    /// <summary>
    /// 원하는 높이의 스폰 시작지점의 위치값을 반환받습니다.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosY posY)
    {
        return GetSpawnerPos(E_SpawnerPosX.START, posY);
    }

    /// <summary>
    /// 기획팀에 제시할 디버깅용 기능입니다.
    /// </summary>
    private void DrawRay(Vector3 startPos,Vector3 dir,Color color)
    {
        Debug.DrawRay(startPos, dir * 50f, color);
    }
}
