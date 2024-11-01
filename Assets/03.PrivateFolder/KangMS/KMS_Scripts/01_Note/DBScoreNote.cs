using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Note;

public class DBScoreNote : Note, IPoolingObj
{
    [SerializeField] private Note upNote;
    [SerializeField] private Note downNote;
    [SerializeField] private GameObject bodyColl;

    private bool _upHit = false;
    private bool _downHit = false;
    private E_NoteDecision _decision;

    public E_Pool MyPoolType => E_Pool.DBSCORE_NOTE;

    public override void Initialize(Vector3 endPoint, float speed, float scoreValue, float damage = 0, float length = 0)
    {
        base.Initialize(endPoint, speed, scoreValue, damage);

        // TotalHeight를 기준으로 upNote와 downNote의 위치 설정
        float halfHeight = GameManager.Director.TotalHeight / 2f;

        // upNote는 위쪽에 배치
        if (upNote != null)
        {
            upNote.transform.position = transform.position + new Vector3(0, halfHeight, 0);
            Vector3 upNoteEndPoint = new Vector3(endPoint.x, upNote.transform.position.y, endPoint.z);
            upNote.Initialize(upNoteEndPoint, speed, scoreValue, damage);
        }

        // downNote는 아래쪽에 배치
        if (downNote != null)
        {
            downNote.transform.position = transform.position - new Vector3(0, halfHeight, 0);
            Vector3 downNoteEndPoint = new Vector3(endPoint.x, downNote.transform.position.y, endPoint.z);
            downNote.Initialize(downNoteEndPoint, speed, scoreValue, damage);
        }

        // bodyColl의 위치는 중간에 배치 (DBScoreNote의 기본 위치와 동일)
        if (bodyColl != null)
        {
            bodyColl.transform.position = transform.position;

            // bodyColl의 y축 크기를 두 노트 사이의 거리로 설정
            Vector3 bodyScale = bodyColl.transform.localScale;
            bodyColl.transform.localScale = new Vector3(bodyScale.x, halfHeight*2, bodyScale.z);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        Initialize(endPoint, speed, scoreValue);

    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //        OnHit(E_NoteDecision.Perfect, E_Boutton.F_BOUTTON);

    //    if (Input.GetKeyDown(KeyCode.D))
    //        OnHit(E_NoteDecision.Perfect, E_Boutton.J_BOUTTON);

    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        Note.isBoss = true;
    //        Debug.Log("boss On");
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        Note.isBoss = false;
    //        Debug.Log("boss Off");
    //    }
    //}
    
    public override float GetDamage()
    {
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {

        if (button == E_Boutton.F_BOUTTON)
        {
            _upHit = true;
            Debug.Log("F On");

            if (_upHit && _downHit)
            {
                DBHit(_decision, button);
            }

            upNote.OnHit(decision, E_Boutton.None);
        }
        if (button == E_Boutton.J_BOUTTON)
        {
            _downHit = true;
            Debug.Log("J On");
            
            if (_upHit && _downHit)
            {
                DBHit(_decision, button);
            }
            
            downNote.OnHit(decision, E_Boutton.None);
        }
    } 

    private void DBHit(E_NoteDecision decision, E_Boutton button)
    {
        Debug.Log("Hit On");

        if (!_isHit)
        {
            _isHit = true;
            CalculateScore(decision);
            ShowEffect();
            
            gameObject.SetActive(false);
            Debug.Log("DB노트 사라짐.");

            if (button == E_Boutton.F_BOUTTON)
                upNote.OnHit(decision, E_Boutton.None);
            else if (button == E_Boutton.J_BOUTTON)
                downNote.OnHit(decision, E_Boutton.None);

            ReturnToPool();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    /// <summary>
    /// NoteMediator에서 등록된 노트를 제거하고 objPool로 반환.
    /// </summary>
    public override void ReturnToPool()
    {
        GameManager.Mediator.Unregister(this);
        Return();
    }
}
