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
        _duration = 8f;
        _time = 0;
        _t = 0;
        _destination = GameManager.NoteDirector.GetBossPoses(E_SpawnerPosY.MIDDLE);
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
    private const float MIN_MOVE_WAIT_TIME = 4;
    private const float MAX_MOVE_WAIT_TIME = 8;

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
        _duration = 9f;
        int rand = Random.Range(0, (int)E_SpawnerPosY.E_SpawnerPosY_MAX);
        _destination = GameManager.NoteDirector.GetBossPoses((E_SpawnerPosY)rand);
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
        _time = 0;
        _t = 0;
        _duration = 12f;
        _destination = GameManager.NoteDirector.GetBossPoses(E_SpawnerPosY.MIDDLE);
    }

    public void Exit()
    {
        _boss.SetState(_boss.RushState);
    }

    public void Update()
    {
        if (_t >= 1)
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
    private Vector3 _destination;

    public void Enter()
    {
        _time = 0;
        _t = 0;
        _duration = 3f;
        _destination = new Vector3(-5, 0, 0);
    }

    public void Exit()
    {
        _boss.SetState(_boss.ClosedPlayerState);
    }

    public void Update()
    {
        if (_t >= 1)
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

public class BossClosedPlayer : BossState, IState
{
    public BossClosedPlayer(BossController boss) : base(boss)
    {
    }

    public BossClosedPlayer(BossController boss, float shakeDuration) : base(boss)
    {
        this._duration = shakeDuration;
    }

    // 흔들림 모션, 카메라 줌?
    private float _shakePower = 2f;
    private float _duration;
    private float _timer;
    private Vector3 _initPos;
    private Vector3 _randPos;

    public void Enter()
    {
        _timer = 0;
        _initPos = _boss.transform.position;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if(_timer >= _duration)
        {
            _boss.SetState(_boss.MoveState);
            return;
        }

        _randPos = new Vector3(Random.Range(_shakePower * -1, _shakePower),
            Random.Range(_shakePower * -1, _shakePower));

        _boss.transform.position = _randPos;

        _timer += Time.deltaTime;
    }
}

public class BossDead : BossState, IState
{
    public BossDead(BossController boss) : base(boss)
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

public class BossRecover : BossState, IState
{
    public BossRecover(BossController boss) : base(boss)
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

