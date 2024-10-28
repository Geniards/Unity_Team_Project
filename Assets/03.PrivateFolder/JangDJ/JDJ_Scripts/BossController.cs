using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossStat _stat = new BossStat();
    [SerializeField] private BossMovement _movement = null;

    private E_SpawnerPosY _curPos = E_SpawnerPosY.NONE;
    private E_SpawnerPosY _nextPos = E_SpawnerPosY.NONE;

    private E_BossState _state = E_BossState.NONE;
    public E_BossState State
    {
        set
        {
            _state = value;

            switch (_state)
            {
                case E_BossState.IDLE:
                    OnIdle();
                    break;
                case E_BossState.MOVE:
                    OnMove();
                    break;
                case E_BossState.ATTACK:
                    OnAttack();
                    break;
                case E_BossState.RUSHREADY:
                    OnRushReady();
                    break;
                case E_BossState.RUSH:
                    OnRush();
                    break;
                case E_BossState.DEAD:
                    OnDead();
                    break;
                case E_BossState.RECOVER:
                    OnRecover();
                    break;
                case E_BossState.ONDAMAGED:
                    OnDamaged();
                    break;
            }
        }
    }

    private void OnIdle()
    {

    }

    private IEnumerator SelectNextMoveY()
    {
        int rand;

        while (true)
        {
            rand = Random.Range(0, (int)E_SpawnerPosY.E_SpawnerPosY_MAX);

            if(_nextPos != (E_SpawnerPosY)rand)
            {

            }
        }
    }

    private void OnMove()
    {

    }

    private void OnAttack()
    {

    }

    private void OnRushReady()
    {

    }

    private void OnRush()
    {

    }

    private void OnDead()
    {

    }

    private void OnRecover()
    {

    }

    private void OnDamaged()
    {

    }
}
