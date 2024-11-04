using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField, Range(0, 5f)] private float _prevAirTime;  // 캐릭터와 판정원 거리에 따른 추가 체공시간
    [SerializeField, Range(0, 5f)] private float _inAirTime;  // 체공시간 => duration 가정
    [SerializeField, Range(0, 1f)] private float _cantMoveTime;  // 행동불능 시간

    [SerializeField] private float _curHp;
    [SerializeField] private float _clikerTime;                 // 깜빡임 속도
    [SerializeField] private float _invincivilityTime;          // 무적시간
    [SerializeField] private float _playerOutTime;              // 플레이어가 화면 밖으로 나가는 시간
    [SerializeField, Range(-3, 3)] private float _jumpPosOffsetDist;

    [Header("참조")]
    [SerializeField] private Animator _anim = null;
    [SerializeField] private WGH_AreaJudge _judge = null;

    [Header("Ray")]
    [SerializeField] private LayerMask _collLayer;

    [SerializeField] private Vector2 _topRayOffset;
    [SerializeField] private Vector2 _botRayOffset;
    [SerializeField] private Vector2 _rightRayOffset;

    [SerializeField] private Vector2 _topBoxSize;
    [SerializeField] private Vector2 _botBoxSize;
    [SerializeField] private Vector2 _rightBoxSize;

    private float _time;
    private float _velocity;
    private float _deltaY;
    private Vector3 _initPos;
    public Vector3 _jumpPos { get; private set; }
    private Vector3 _collapsPos;


    public float CurHP
    {
        get { return _curHp; }
        private set
        {
            _curHp = value;
            if (_curHp != DataManager.Instance.PlayerMaxHP)
                DataManager.Instance.UpdatePlayerHP(_curHp);

            if (value == 0)
                StartCoroutine(Die());
        }
    }

    private Vector3 _bossApproachPos;
    public Vector3 StartPos { get; private set; }

    private bool _isFPress;                                    // f 입력 여부
    private bool _isJPress;                                    // j 입력 여부
    private float _fPressTime;                                 // f 입력 시간을 받을 값
    private float _jPressTime;                                 // j 입력 시간을 받을 값
    private float _approachDur;                                // 접근까지 걸리는 시간
    private float _contactDur;                                 // 난투 시간
    private float _mass => 10f;
    private float _gForce => 9.81f;
    private int _meleeCount;                                   // 난투 필요 타격 횟수

    public bool IsDied { get; private set; }           // 사망여부
    public bool IsDamaged { get; private set; }        // 피격 여부
    public bool IsCanMove { get; private set; }        // 공격 가능 여부
    public bool IsAir { get; private set; }            // 체공 여부
    public bool IsContact { get; private set; }        // 난투 중인지 여부

    private void Awake()
    {
        CurHP = DataManager.Instance.PlayerMaxHP;
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        _jumpPos = transform.position + Vector3.up * (GameManager.Director.GetCheckPoses(E_SpawnerPosY.TOP).y + _jumpPosOffsetDist);
        EventManager.Instance.AddAction(E_Event.BOSSRUSH, ApproachBoss, this);
        EventManager.Instance.AddAction(E_Event.ENTERCONTACT, ContactBoss, this);
        EventManager.Instance.AddAction(E_Event.CONTACTEND, ContactEnd, this);
        _bossApproachPos = DataManager.Instance.ContactPos + new Vector3(-0.8f, -1, 0);
        StartPos = transform.position;
        _judge = FindAnyObjectByType<WGH_AreaJudge>();
        _approachDur = DataManager.Instance.ApproachDuration; // 임시 0.2
        _contactDur = DataManager.Instance.ContactDuration; // 임시 4
        _meleeCount = DataManager.Instance.MeleeCount; // 임시 2
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, PlayerOut, this);
        IsCanMove = true;
        _initPos = transform.position;
        _collapsPos = _jumpPos;

        _inAirTime = GameManager.Director.GetBPMtoIntervalSec() / DataManager.Instance.SelectedStageData.NoteSpeed;

        float dist = GameManager.Director.GetCheckPoses(E_SpawnerPosY.BOTTOM).x - transform.position.x;

        _prevAirTime = dist / DataManager.Instance.SelectedStageData.NoteSpeed;
    }

    /// <summary>
    /// 클리어 시 체력비례 점수 메서드
    /// </summary>
    public int GetHpScore()
    {
        return (int)(CurHP * 100);
    }

    private Collider2D _coll;

    private void Update()
    {
        // 테스트용
        //if(Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    StartCoroutine(PlayerOutMove());
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    EventManager.Instance.PlayEvent(E_Event.BOSSRUSH);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    EventManager.Instance.PlayEvent(E_Event.ENTERCONTACT);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    EventManager.Instance.PlayEvent(E_Event.CONTACTEND);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    CurHP -= 100;
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Note.isBoss = true;
        //}
        DebugBoxRay(_topRayOffset, _topBoxSize, Color.blue);
        DebugBoxRay(_botRayOffset, _botBoxSize, Color.green);
        DebugBoxRay(_rightRayOffset, _rightBoxSize, Color.red);

        if (RayCheck(_topRayOffset, _topBoxSize, out _coll))
        {
            CheckNoteContact();
        }
        else if (RayCheck(_rightRayOffset, _rightBoxSize, out _coll))
        {
            // 전방 충돌일 경우
            CheckNoteContact();
        }
        else if (RayCheck(_botRayOffset, _botBoxSize, out _coll) || IsAir == false)
        {
            // 하단 충돌일 경우 2개
            CheckNoteContact();
            CheckGroundContact();
        }

        Fall();
    }

    public void ResetCollapsPos()
    {
        _collapsPos = _jumpPos;
        _velocity = 0;
        _time = 0;
    }
    // 낙하
    public void Fall()
    {
        if (!IsAir)
            return;

        _time += Time.deltaTime;

        // 목표 지점까지 가기 위해 필요한 가속도 계산
        float duration = (_inAirTime + _prevAirTime) * 2;

        float distance = _jumpPosOffsetDist - _initPos.y;
        
        float gravity = (2 * distance) / (duration * duration); // 가속도 계산

        // 시간에 따른 변위 계산
        _deltaY = 0.5f * gravity * _time * _time; // 가속도에 의한 이동 거리
        
        // 현재 위치 업데이트
        transform.position = _collapsPos - new Vector3(0, _deltaY, 0);
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = _jumpPos;
            _time = 0;
            return;
        }

        // 목표 시간에 도달하면 위치를 초기화
        if (_time >= duration)
        {
            transform.position = _initPos;
            _time = 0f; // 시간 초기화
        }
    }

    private void CheckGroundContact()
    {
        if (_coll != null && _coll.gameObject.layer == 16 && IsAir == true)
        {
            _anim.SetBool("IsAir", false);
            //SetAnim("Run");
            IsAir = false;

            Debug.Log(Time.time);

            _coll = null;
        }
    }

    private void CheckNoteContact()
    {
        // 상단 충돌일 경우
        if (_coll != null && _coll.TryGetComponent<Note>(out Note note) && !IsDamaged && !IsDied)
        {
            _judge.SetComboReset();
            SetAnim("OnDamage");
            IsDamaged = true;
            IsCanMove = false;
            float _dmg = note.GetDamage();
            
            if (!Note.isBoss)
            { CurHP -= _dmg; }
            else
            { CurHP -= _dmg * 2; }
            
            StartCoroutine(CantMoveTime());
            StartCoroutine(Invincibility());
            StartCoroutine(Clicker());

            _coll = null;
        }

    }

    public void JumpMove()
    {
        if(IsAir)
        {
            SetAnim("Jump");
            ResetCollapsPos();
            transform.position = _jumpPos;
        }
    }

    // 플레이어 위치
    public bool RayCheck(Vector2 offset, Vector2 size, out Collider2D hit)
    {
        Vector2 boxCenter = transform.position + new Vector3(offset.x, offset.y, 0);
        Vector2 topLeft = boxCenter + new Vector2((size.x / 2) * -1, (size.y / 2));
        Vector2 botRight = new Vector2(topLeft.x + size.x, topLeft.y - size.y);

        hit = Physics2D.OverlapArea(topLeft, botRight, _collLayer.value);

        if (hit == null)
            return false;

        return true;
    }

    public void DebugBoxRay(Vector2 offset, Vector2 size, Color color)
    {
        Vector2 boxCenter = transform.position + new Vector3(offset.x, offset.y, 0);

        Vector2 topLeft = boxCenter + new Vector2((size.x / 2) * -1, (size.y / 2));

        //상
        Debug.DrawRay(topLeft, Vector3.right * size.x, color);
        Debug.DrawRay(topLeft + Vector2.down * size.y, Vector3.right * size.x, color);

        //좌
        Debug.DrawRay(topLeft, Vector3.down * size.y, color);
        Debug.DrawRay(topLeft + Vector2.right * size.x, Vector3.down * size.y, color);
    }


    private void PlayerOut()
    {
        StartCoroutine(PlayerOutMove());
    }
    IEnumerator PlayerOutMove()
    {
        float _time = 0;
        while (true)
        {
            _time += Time.deltaTime;
            float t = (_time / _playerOutTime) / 10;
            if (_time < _playerOutTime)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + 
                    Vector3.right * GameManager.Director.GetStartSpawnPoses(E_SpawnerPosY.BOTTOM).x, t);
            }
            else
            {
                _judge.SetComboReset();
                EventManager.Instance.PlayEvent(E_Event.STAGE_END);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator Die()
    {
        
        DataManager.Instance.SetStageClear(false);

        IsDied = true;

        SetAnim("Die");
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.PlayEvent(E_Event.PLAYERDEAD);
        StartCoroutine(DieMove());
        yield return new WaitForSeconds(1f);
        EventManager.Instance.PlayEvent(E_Event.STAGE_END);
        yield break;
    }
    IEnumerator DieMove()
    {
        float _time = 0;
        while (true)
        {
            _time += Time.deltaTime;
            float t = (_time / _playerOutTime) / 10;
            if (_time < _playerOutTime)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position +
                    Vector3.left * 3, t);
            }
            else
            {
                _judge.SetComboReset();
                yield break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 보스 직면 메서드
    /// </summary>
    private void ApproachBoss()
    {
        SetAnim("ConfrontBoss");
        StartCoroutine(ApproachMove());
    }
    IEnumerator ApproachMove()
    {
        _judge.enabled = false;
        float _time = 0;

        while (true)
        {
            _time += Time.deltaTime;
            float t = _time / _approachDur;
            if (transform.position != _bossApproachPos)
            {
                transform.position = Vector3.Lerp(transform.position, _bossApproachPos, t);
            }
            else if (transform.position == _bossApproachPos)
            {
                yield break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 보스 직면 끝
    /// </summary>
    private void ContactEnd()
    {
        StartCoroutine(EndMelee());
    }
    IEnumerator EndMelee()
    {
        if (_meleeCount <= 0)
        {
            DataManager.Instance.Boss.GetMeleeResult(true);
        }
        else
        {
            DataManager.Instance.Boss.GetMeleeResult(false);
            _judge.SetComboReset();
            CurHP -= 50;
            Debug.Log("보스 난투 격파 실패");
        }
        
        
        float _time = 0;
        SetAnim("Run");
        while (true)
        {
            _time += Time.deltaTime;
            float t = _time / _approachDur;
            if (_time < _approachDur)
            {
                transform.position = Vector3.Lerp(transform.position, StartPos, t);
            }
            else
            {
                _judge.enabled = true;
                transform.position = StartPos;
                yield break;
            }
            yield return null;
        }
        
    }
    /// <summary>
    /// 보스 난투 메서드
    /// </summary>
    private void ContactBoss()
    {
        StartCoroutine(Melee());
    }
    IEnumerator Melee()
    {
        float _contactTime = 0;
        while (true)
        {
            _contactTime += Time.deltaTime;
            if (_contactTime < _contactDur)
            {
                // 난투
                if (Input.GetKeyDown(KeyCode.J))
                {
                    _meleeCount--;
                    SetAnim("GroundAttack");
                    _judge.AddCombo();
                    _judge.AddPerfectCount();
                    _judge._FloatCombo.SpawnCombo(_judge.Combo);
                    DataManager.Instance.AddScore(Mathf.RoundToInt(10 * 2 * ((_judge.CheckCurCombo() * 0.1f) + 1)));
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    _meleeCount--;
                    SetAnim("JumpAttack");
                    _judge.AddCombo();
                    _judge.AddPerfectCount();
                    _judge._FloatCombo.SpawnCombo(_judge.Combo);
                    DataManager.Instance.AddScore(Mathf.RoundToInt(10 * 2 * ((_judge.CheckCurCombo() * 0.1f) + 1)));
                }
                else if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.J))
                {
                    SetAnim("ConfrontBoss");
                }
            }
            else
            {
                yield break;
            }
            yield return null;
        }
    }
    // 캐릭터 피격 깜빡거림
    IEnumerator Clicker()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(_clikerTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(_clikerTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(_clikerTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(_clikerTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(_clikerTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);

        yield break;
    }
    // 무적
    IEnumerator Invincibility()
    {
        SetAnim("Run");
        yield return new WaitForSeconds(_invincivilityTime);
        IsDamaged = false;
        yield break;
    }

    // 행동불가
    IEnumerator CantMoveTime()
    {
        yield return new WaitForSeconds(_cantMoveTime);
        IsCanMove = true;
        yield break;
    }

    /// <summary>
    /// 애니메이션 시작 메서드
    /// </summary>
    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
    /// <summary>
    /// 체공여부 조절 메서드
    /// </summary>
    public void IsAirControl(bool state)
    {
        if (state)
        {
            IsAir = true;
            _anim.SetBool("IsAir", true);
        }
            
        if (!state)
            IsAir = false;
    }
}
