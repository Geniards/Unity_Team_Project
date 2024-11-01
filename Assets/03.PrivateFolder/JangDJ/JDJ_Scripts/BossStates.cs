using UnityEngine;

public abstract class BossState
{
    protected BossController _boss;

    public BossState(BossController boss)
    { this._boss = boss; }
}

public class BossIntoField : BossState, IState
{
    public BossIntoField(BossController boss) : base(boss)
    {
    }

    private Vector3 _destination;
    private float _duration;
    private float _time;
    private float _t;

    public void Enter()
    {
        _duration = 10f;
        _time = 0;
        _t = 0;
        _destination = GameManager.Director.GetBossPoses(E_SpawnerPosY.BOTTOM);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if(_t >= 1)
        {
            _boss.SetState(_boss.IdleState);
            return;
        }

        _time += Time.deltaTime;
        _t = Mathf.Clamp01(_time / _duration);

        _boss.transform.position
                = Vector3.Slerp(_boss.transform.position, _destination, _t);
    }
}

public class BossIdle : BossState, IState
{
    private const float MIN_MOVE_WAIT_TIME = 2;
    private const float MAX_MOVE_WAIT_TIME = 5;

    public BossIdle(BossController boss) : base(boss)
    {
    }

    private float _waitTime;
    private float _timer;

    public void Enter()
    {
        _waitTime = Random.Range(MIN_MOVE_WAIT_TIME, MAX_MOVE_WAIT_TIME);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if(_waitTime <= _timer)
        {
            _boss.SetState(_boss.MoveState);
            return;
        }
        
        _timer += Time.deltaTime;
    }
}

public class BossMove : BossState, IState
{
    public BossMove(BossController boss) : base(boss)
    {
    }

    private Vector3 _destination;
    private float _duration;
    private float _time;
    private float _t;

    public void Enter()
    {
        _time = 0;
        _t = 0;
        _duration = 4f;
        //int rand = Random.Range(0, (int)E_SpawnerPosY.E_SpawnerPosY_MAX);
        _destination = GameManager.Director.GetBossPoses((E_SpawnerPosY.BOTTOM));
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if(_t >= 1)
        {
            _boss.SetState(_boss.IdleState);
            return;
        }

        _time += Time.deltaTime;
        _t = Mathf.Clamp01(_time / _duration);

        _boss.transform.position =
            Vector3.Lerp(_boss.transform.position, _destination, _t);
    }
}

// 미정
public class BossAttack : BossState, IState
{
    public BossAttack(BossController boss) : base(boss)
    {
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class BossRushReady : BossState, IState
{
    public BossRushReady(BossController boss) : base(boss)
    {
    }

    private float _time;
    private float _t;
    private float _duration;
    private Vector3 _destination;

    public void Enter()
    {
        _boss.Anim.PlayRushReadyAnim();
        EventManager.Instance.PlayEvent(E_Event.NOTE_CLEAR);
        EventManager.Instance.PlayEvent(E_Event.SPAWN_STOP);
        
        _time = 0;
        _t = 0;
        _duration = 2f;
        _destination = GameManager.Director.GetBossPoses(E_SpawnerPosY.MIDDLE);
    }

    public void Exit()
    {
        _boss.SetRushReady(false);
    }

    public void Update()
    {
        if (_t >= 1)
        {
            _boss.SetState(_boss.RushState);
            return;
        }

        _time += Time.deltaTime;
        _t = Mathf.Clamp01(_time / _duration);

        _boss.transform.position =
            Vector3.Lerp(_boss.transform.position, _destination, _t);
    }
}

/// <summary>
/// 박자에 맞게 도착해야함
/// </summary>
public class BossRush : BossState, IState
{
    public BossRush(BossController boss) : base(boss)
    {
    }

    private float _time;
    private float _t;
    private float _duration;
    private Vector3 _startPosition;
    private Vector3 _destination;
    private Vector3 _offset = Vector3.right * 0.3f;

    public void Enter()
    {
        EventManager.Instance.PlayEvent(E_Event.BOSSRUSH);
        SoundManager.Instance.FadeBGM(false, 2f, 0.01f);
        _time = 0;
        _t = 0;
        _duration = DataManager.Instance.ApproachDuration;
        _startPosition = _boss.transform.position;
        _destination = DataManager.Instance.ContactPos + _offset;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (_time >= _duration)
        {
            _boss.transform.position = _destination;
            SoundManager.Instance.PlaySFX(E_SFX.ENTER_CONTACT);
            _boss.SetState(_boss.ClosedPlayerState);
            return;
        }

        _time += Time.deltaTime;
        float t = _time / _duration;

        _boss.transform.position = Vector3.Lerp(_startPosition, _destination, t);
    }
}

public class BossClosedPlayer : BossState, IState
{
    public BossClosedPlayer(BossController boss) : base(boss)
    {
    }

    public BossClosedPlayer(BossController boss, float shakeDuration) : base(boss)
    {
        this._duration = shakeDuration;
        _cam = Camera.main.GetComponent<CamMovement>();
    }

    // 흔들림 모션, 카메라 줌?
    private float _shakePower = 0.14f;
    private float _duration;
    private float _timer;
    private Vector3 _initPos;
    private Vector3 _randPos;
    private CamMovement _cam;

    public void Enter()
    {
        EventManager.Instance.PlayEvent(E_Event.ENTERCONTACT);
        _timer = 0;
        _initPos = _boss.transform.position;
        _cam.Move(_initPos + Vector3.forward * -10, 0.07f);
        _cam.ZoomIn(0.1f,3.6f);
        
    }

    public void Exit()
    {
        _cam.Move(new Vector3(0, 1, -10f), 0.07f);
        _cam.ZoomIn(0.1f, 5f);
        SoundManager.Instance.PlaySFX(E_SFX.EXIT_CONTACT);
        SoundManager.Instance.FadeBGM(true, 2f);
    }

    public void Update()
    {
        if(_timer >= _duration)
        {
            EventManager.Instance.PlayEvent(E_Event.CONTACTEND);
            return;
        }

        _randPos = new Vector3(Random.Range(_shakePower * -1, _shakePower),
            Random.Range(_shakePower * -1, _shakePower));

        _randPos += _initPos;

        _boss.transform.position = _randPos;

        _timer += Time.deltaTime;
    }
}

public class BossDead : BossState, IState
{
    public BossDead(BossController boss) : base(boss)
    {
    }

    private float _time;
    private float _t;
    private float _duration;
    private Vector3 _startPosition;
    private Vector3 _destination;

    public void Enter()
    {
        _boss.Anim.PlayBossDefeatAnim();
        _time = 0;
        _t = 0;
        _duration = 0.2f;
        _startPosition = _boss.transform.position;
        _destination = GameManager.Director.GetStartSpawnPoses(E_SpawnerPosY.BOTTOM);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (_time >= _duration)
        {
            SoundManager.Instance.SetBGMVolume(DataManager.Instance.BGMVolume/2);
            _boss.transform.position = _destination;
            SoundManager.Instance.FadeBGM(false, 10f, 0.02f);
            return;
        }

        _time += Time.deltaTime;
        float t = _time / _duration;

        _boss.transform.position = Vector3.Lerp(_startPosition, _destination, t);
    }
}

public class BossRecover : BossState, IState
{
    public BossRecover(BossController boss) : base(boss)
    {
    }
    private float _time;
    private float _t;
    private float _duration;
    private Vector3 _startPosition;
    private Vector3 _destination;

    public void Enter()
    {
        _boss.Anim.PlayBossWinAnim();
        EventManager.Instance.PlayEvent(E_Event.SPAWN_START);
        _boss.Heal();

        _time = 0;
        _t = 0;
        _duration = 0.2f;
        _startPosition = _boss.transform.position;
        _destination = GameManager.Director.GetBossPoses(E_SpawnerPosY.BOTTOM);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (_time >= _duration)
        {
            _boss.transform.position = _destination;
            _boss.SetState(_boss.MoveState);
            return;
        }

        _time += Time.deltaTime;
        float t = _time / _duration;

        _boss.transform.position = Vector3.Lerp(_startPosition, _destination, t);
    }
}

