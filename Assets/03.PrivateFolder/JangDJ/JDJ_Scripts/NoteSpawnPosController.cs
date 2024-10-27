using System.Collections.Generic;
using Unity.VisualScripting;
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

    //[Space(20f)]
    //[SerializeField] private float _startPosOffset;

    private List<double> _posesXvalues = null; // end , check , start
    private List<double> _posesYvalues = null; // bot , mid , top

    // ���� ������������ ���� üũ����Ʈ������ �Ÿ���
    public double DistSpawnToCheck =>
         _posesXvalues[(int)E_SpawnerPosX.START] - _posesXvalues[(int)E_SpawnerPosX.CHECK];

    private bool _lock;

    private double under = double.MaxValue;
    private double over = double.MinValue;

    [SerializeField] GameObject minobj;
    [SerializeField] GameObject maxobj;

    private void Awake()
    {
        _posesXvalues = new List<double>();
        _posesYvalues = new List<double>();

        //_startPos.position = _startPos.position + Vector3.right * _startPosOffset;
        _startPos.position = _checkPos.position + Vector3.right * 20;

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
        if (Input.GetKeyDown(KeyCode.Space))
            _lock = true;

        for (int i = 0; i < _posesYvalues.Count; i++)
        {
            DrawRay(new Vector3(_startPos.position.x, (float)_posesYvalues[i]),
                Vector3.left, Color.green);
        }

        for (int i = 0; i < _posesXvalues.Count; i++)
        {
            DrawRay(new Vector3((float)_posesXvalues[i], (float)_posesYvalues[_posesYvalues.Count-1]),
                Vector3.down, Color.red);
        }
    }

    /// <summary>
    /// ���ϴ� �� ������ ��ġ���� ��ȯ �޽��ϴ�.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosX posX, E_SpawnerPosY posY)
    {
        return new Vector3((float)_posesXvalues[(int)posX], (float)_posesYvalues[(int)posY]);
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

    public void NoteCheckRay()
    {
        RaycastHit hit;
        Physics.Raycast(_checkPos.position,Vector3.down, out hit , 50f);
        Debug.DrawRay(_checkPos.position, Vector3.down * 60f, Color.blue,0.03f);

        if(hit.collider != null)
        {
            double dist = _checkPos.position.x - hit.transform.position.x;

            if (dist < under)
            {
                under = dist;
                minobj.transform.position = new Vector3((float)under + _checkPos.position.x, minobj.transform.position.y);
            }
                

            if (dist > over)
            {
                over = dist;
                maxobj.transform.position = new Vector3((float)over + _checkPos.position.x, maxobj.transform.position.y);
            }
                

            //Debug.Log($"���� �������� : {under} , �ְ� �������� {over}");
            //Debug.Log(Time.time);

            if (_lock)
                Time.timeScale = 0;
        }
        
    }
}
