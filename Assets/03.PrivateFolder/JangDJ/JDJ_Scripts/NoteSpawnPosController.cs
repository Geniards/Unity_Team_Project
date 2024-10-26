using System.Collections.Generic;
using UnityEngine;

public class NoteSpawnPosController : MonoBehaviour
{
    [Header("�������� ���� X ���� �����ϱ� ���� Ʈ������")]
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _checkPos;
    [SerializeField] private Transform _endPos;

    [Space(20f), Header("�������� ���� Y ���� �����ϱ� ���� Ʈ������")] 
    [SerializeField] private Transform _bottomPointPos;
    [SerializeField] private Transform _bottomToIntervalSpawners;

    private List<float> _posesXvalues = null; // end , check , start
    private List<float> _posesYvalues = null; // bot , mid , top

    // ���� ������������ ���� üũ����Ʈ������ �Ÿ���
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
    /// ���ϴ� �� ������ ��ġ���� ��ȯ �޽��ϴ�.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosX posX, E_SpawnerPosY posY)
    {
        return new Vector3(_posesXvalues[(int)posX], _posesYvalues[(int)posY]);
    }

    /// <summary>
    /// ���ϴ� ������ ���� ���������� ��ġ���� ��ȯ�޽��ϴ�.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosY posY)
    {
        return GetSpawnerPos(E_SpawnerPosX.START, posY);
    }

    /// <summary>
    /// ��ȹ���� ������ ������ ����Դϴ�.
    /// </summary>
    private void DrawRay(Vector3 startPos,Vector3 dir,Color color)
    {
        Debug.DrawRay(startPos, dir * 50f, color);
    }
}
