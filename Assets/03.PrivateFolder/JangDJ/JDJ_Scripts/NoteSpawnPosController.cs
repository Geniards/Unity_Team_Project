using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteSpawnPosController : MonoBehaviour
{
    [Header("��Ʈ�� ������ ��ġ Y ��ǥ ����")]
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _checkPos;
    [SerializeField] private Transform _bossPos;
    [SerializeField] private Transform _endPos;

    [Space(20f), Header("���Ұ� �߰� ������ġ X ��ǥ ����")] 
    [SerializeField] private Transform _bottomPointPos;
    [SerializeField] private Transform _bottomToIntervalSpawners;

    private List<double> _posesXvalues = null; // end , check , start
    private List<double> _posesYvalues = null; // bot , mid , top

    // �����������κ��� üũ����Ʈ������ �Ÿ�
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

        SetSpawnPos();
        
        RegistXValues();
        RegistYValues();

        CheckCorrectPos();
    }

    private void SetSpawnPos()
    {
        float minOffsetX = 20f;
        int speed = DataManager.Instance.GameSpeed;

        for (int i = 1; i < 30; i++)
        {
            if(speed * i > minOffsetX)
            {
                _startPos.position = 
                    _checkPos.position + Vector3.right * speed * i;

                break;
            }
        }
    }

    private void CheckCorrectPos()
    {
        bool IsInCheck(Vector3 pos)
        {
            Vector3 tempPos = Camera.main.WorldToViewportPoint(pos);

            if (0 <= tempPos.x && tempPos.x <= 1)
            {
                if (0 <= tempPos.y && tempPos.y <= 1)
                {
                    return true;
                }
            }

            return false;
        }

        if(IsInCheck(_endPos.position) == true)
        { throw new System.Exception("End ��ǥ�� ȭ�� ���� ���� ��ġ��"); }
        else if(IsInCheck(_checkPos.position) == false)
        { throw new System.Exception("Check ��ǥ�� ȭ�� ���� �ܿ� ��ġ��"); }
        else if (IsInCheck(_bossPos.position) == false)
        { throw new System.Exception("BossStay ��ǥ�� ȭ�� ���� �ܿ� ��ġ��"); }
        else if (IsInCheck(_startPos.position) == true)
        { throw new System.Exception("Start ��ǥ�� ȭ�� ���� ���� ��ġ��"); }
        else if (IsInCheck(_bottomPointPos.position) == false)
        { throw new System.Exception("BotPoint ��ǥ�� ȭ�� ���� �ܿ� ��ġ��"); }
        else if (IsInCheck(_bottomToIntervalSpawners.position) == false)
        { throw new System.Exception("BotToInterval ��ǥ�� ȭ�� ���� �ܿ� ��ġ��"); }

        for (int i = _posesXvalues.Count-2; i >= 0; i--)
        {
            if (_posesXvalues[i] < _posesXvalues[i+1])
            {   
                throw new System.Exception($"" +
                    $"{(E_SpawnerPosX)(i + 1)} ��ġ�� {(E_SpawnerPosX)(i)} ���� �տ� ��ġ�Ǿ����ϴ�.");
            }
        }

        for (int i = 0; i < _posesYvalues.Count-2; i++)
        {
            if (_posesYvalues[i] > _posesYvalues[i+1])
            {
                throw new System.Exception($"" +
                    $"{(E_SpawnerPosY)(i)} ��ġ�� {(E_SpawnerPosY)(i+1)} ���� ���� ��ġ�Ǿ����ϴ�.");
            }
        }
    }

    private void RegistXValues()
    {
        _posesXvalues.Add(_startPos.position.x);
        _posesXvalues.Add(_bossPos.position.x);
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
        //if (Input.GetKeyDown(KeyCode.Space))
        //    _lock = true;

        for (int i = 0; i < _posesYvalues.Count; i++)
        {
            DrawRay(new Vector3(_startPos.position.x, (float)_posesYvalues[i]),
                Vector3.left, Color.green);
        }

        for (int i = 0; i < _posesXvalues.Count; i++)
        {
            DrawRay(new Vector3((float)_posesXvalues[i], (float)_posesYvalues[_posesYvalues.Count - 1]),
                Vector3.down, Color.red);
        }
    }

    /// <summary>
    /// ��Ʈ �ý����� �ٽ� ��ǥ���� Ȯ���մϴ�.
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosX posX, E_SpawnerPosY posY)
    {
        return new Vector3((float)_posesXvalues[(int)posX], (float)_posesYvalues[(int)posY]);
    }

    /// <summary>
    /// ���� ��ǥ�� Ȯ���ϴ� ���
    /// </summary>
    public Vector3 GetSpawnerPos(E_SpawnerPosY posY)
    {
        return GetSpawnerPos(E_SpawnerPosX.START, posY);
    }

    /// <summary>
    /// ����׿�
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
                

            //Debug.Log($"���� �Ÿ� : {under} , �ְ� �Ÿ� {over}");
            //Debug.Log(Time.time);

            //if (_lock)
            //    Time.timeScale = 0;
        }
        
    }
}
