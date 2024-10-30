using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Note;

public class DBScoreNote : Note, IPoolingObj
{
    [SerializeField] private ScoreNote upNote;
    [SerializeField] private ScoreNote downNote;
    [SerializeField] private GameObject bodyColl;

    private bool _upHit = false;
    private bool _downHit = false;

    public E_Pool MyPoolType => E_Pool.DBSCORE_NOTE;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Initialize(endPoint, speed, scoreValue);

        }
        if (Input.GetKeyDown(KeyCode.S))
            OnHit(E_NoteDecision.Perfect, E_Boutton.F_BOUTTON);

        if (Input.GetKeyDown(KeyCode.D))
            OnHit(E_NoteDecision.Perfect, E_Boutton.J_BOUTTON);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Note.isBoss = true;
            Debug.Log("boss On");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Note.isBoss = false;
            Debug.Log("boss Off");
        }
    }

    public override float OnDamage()
    {
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {
        if (button == E_Boutton.F_BOUTTON)
        {
            upNote.OnHit(decision, E_Boutton.None);
            _upHit = true;
            Debug.Log("F On");
        }
        if (button == E_Boutton.J_BOUTTON)
        {
            downNote.OnHit(decision, E_Boutton.None);
            _downHit = true;
            Debug.Log("J On");
        }

        if (_upHit && _downHit)
            DBHit(decision);
    } 

    private void DBHit(E_NoteDecision decision)
    {
        Debug.Log("Hit On");

        if (!_isHit)
        {
            _isHit = true;
            CalculateScore(decision);
            ShowEffect();
            gameObject.SetActive(false);
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }
}
