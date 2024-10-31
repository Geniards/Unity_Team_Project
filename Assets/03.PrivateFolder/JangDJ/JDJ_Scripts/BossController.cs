using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossStat _stat = new BossStat();

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
        DataManager.Instance.SetBossHP(_stat.Hp);
    }

    private void InitStates()
    {
        IntoField = new BossIntoField(this);
        IdleState = new BossIdle(this);
        AttackState = new BossAttack(this);
        MoveState = new BossMove(this);
        RushState = new BossRush(this);
        RushReadyState = new BossRushReady(this);
        ClosedPlayerState = new BossClosedPlayer(this,5);
        DeadState = new BossDead(this);
        RecoverState = new BossRecover(this);
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

    public void Dead()
    {
        Destroy(this.gameObject); // 임시
    }

    public float OnDamage(float damage)
    {
        float currentHp = _stat.AddHp(damage * -1);
        if (currentHp <= 0)
            { SetState(RushReadyState); }

        return currentHp;
    }

    public void GetMeleeResult(bool result)
    {
        if (result == true)
            SetState(DeadState);
        else
            SetState(RecoverState);
    }

    private void Update()
    {
        if (CurrentState != null)
            CurrentState.Update();
    }
}
