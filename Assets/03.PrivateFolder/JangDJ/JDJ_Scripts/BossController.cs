using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossStat _stat = new BossStat();
    [SerializeField] private BossAnimator _anim;
    public BossAnimator Anim => _anim;

    public IState CurrentState { get; private set; }
    public BossIntoField IntoField;
    public BossIdle IdleState;
    public BossAttack AttackState;
    public BossMove MoveState;
    public BossRush RushState;
    public BossRushReady RushReadyState;
    public BossClosedPlayer ClosedPlayerState;
    public BossDead DeadState;
    public BossRecover RecoverState;

    public BossStat Stat => _stat;
    public int Score => _stat.Score;

    private bool _isRushReady = false;

    public void Initialize()
    {
        InitStates();
        SetInitState(IntoField);
        RegistMyData();
        this.transform.position 
            = GameManager.Director.GetStartSpawnPoses(E_SpawnerPosY.BOTTOM);
    }

    private void RegistMyData()
    {
        DataManager.Instance.SetBossData(this);
    }

    private void InitStates()
    {
        IntoField = new BossIntoField(this);
        IdleState = new BossIdle(this);
        AttackState = new BossAttack(this);
        MoveState = new BossMove(this);
        RushState = new BossRush(this);
        RushReadyState = new BossRushReady(this);
        ClosedPlayerState = new BossClosedPlayer(this, DataManager.Instance.ContactDuration);
        DeadState = new BossDead(this);
        RecoverState = new BossRecover(this);

        _isRushReady = false;
    }

    private void SetInitState(IState state)
    {
        this.CurrentState = state;
        this.CurrentState.Enter();
    }

    public void SetState(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void ReduceScore()
    {
        _stat.ReduceScore();
    }

    public void DeadActionEnd()
    {
        EventManager.Instance.PlayEvent(E_Event.BOSSDEAD);
    }

    public float OnDamage(float damage)
    {
        float currentHp = _stat.AddHp(damage * -1);
        if (currentHp <= 0 && _isRushReady == false)
        {
            _isRushReady = true;
            SetState(RushReadyState); 
        }

        return currentHp;
    }

    public void Heal()
    {
        _stat.AddHp(2); // 임시
    }

    public void SetRushReady(bool value)
    {
        this._isRushReady = value;
    }

    public void GetMeleeResult(bool result)
    {
        Debug.Log(result);

        if (result == true)
            SetState(DeadState);
        else
            SetState(RecoverState);
    }

    private void Update()
    {
        if (CurrentState != null)
            CurrentState.Update();

        //if (Input.GetKeyDown(KeyCode.D))
        //    GetMeleeResult(true);
        //if (Input.GetKeyDown(KeyCode.K))
        //    GetMeleeResult(false);
    }
}
