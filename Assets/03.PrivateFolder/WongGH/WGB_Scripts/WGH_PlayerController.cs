using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField, Range(0, 0.1f)] float _inAirTime;  // 체공시간        
    [SerializeField] float _curHp;
    [SerializeField] float _clikerTime;                 // 깜빡임 속도
    [SerializeField] float _invincivilityTime;          // 무적시간

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    [SerializeField] Note _note;
     
    public float CurHP { get { return _curHp; } private set { _curHp = value; DataManager.Instance.UpdatePlayerHP(_curHp); } }
    public Vector3 PlayerFrontBoss { get; private set; }
    Vector3 _bossApproachPos;
    Vector3 _startPos;

    bool _isFPress;                                    // f 입력 여부
    bool _isJPress;                                    // j 입력 여부
    float _fPressTime;                                 // f 입력 시간을 받을 값
    float _jPressTime;                                 // j 입력 시간을 받을 값
    float _approachDur;
    float _contactDur;
    int _meleeCount;
    public bool IsDied { get; private set; }           // 사망여부
    public bool IsDamaged { get; private set; }        // 피격 여부
    public bool IsAir { get; private set; }            // 체공 여부
    
    private void Awake()
    {
        CurHP = DataManager.Instance.PlayerMaxHP;
        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        

    }
    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.BOSSRUSH, ApproachBoss, this);
        EventManager.Instance.AddAction(E_Event.ENTERCONTACT, ContactBoss, this);
        EventManager.Instance.AddAction(E_Event.CONTACTEND, ContactEnd, this);
        _bossApproachPos = DataManager.Instance.ContactPos + new Vector3(-0.5f, -1, 0);
        _startPos = transform.position;
        _note = FindAnyObjectByType<WGH_AreaJudge>().Note;
        _approachDur = DataManager.Instance.ApproachDuration; // 임시 0.2
        _contactDur = DataManager.Instance.ContactDuration; // 임시 2
        _meleeCount = DataManager.Instance.MeleeCount; // 임시 2
    }
    
    private void Update()
    {
        
        if(Input.GetKey(KeyCode.Alpha1))
        {
            EventManager.Instance.PlayEvent(E_Event.BOSSRUSH);
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            EventManager.Instance.PlayEvent(E_Event.CONTACTEND);
        }
        if (_curHp <= 0 && !IsDied)
        {
            StartCoroutine(Die());
        }
    }
    /// <summary>
    /// 보스 직면 메서드
    /// </summary>
    private void ApproachBoss()
    {
        float _time = 0;
        SetAnim("ConfrontBoss");
        
        _time += Time.deltaTime;
        float t = _time / _approachDur;
        transform.position = Vector3.Lerp(transform.position, _bossApproachPos, t);
        _rigid.isKinematic = true;
    }

    /// <summary>
    /// 보스 직면 끝
    /// </summary>
    private void ContactEnd()
    {
        if (_meleeCount <= 0)
        {
            DataManager.Instance.Boss.GetMeleeResult(true);
        }
        else
        {
            DataManager.Instance.Boss.GetMeleeResult(false);
        }

        float _time = 0;
        SetAnim("Run");

        _time += Time.deltaTime;
        float t = _time / _approachDur;
        transform.position = Vector3.Lerp(transform.position, _startPos, t);
        _rigid.isKinematic = false;
    }
    /// <summary>
    /// 보스 난투 메서드
    /// </summary>
    private void ContactBoss()
    {
        float _contactTime = 0;
        if (_contactTime < _contactDur)
        {
            // 난투
            if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
            {
                _meleeCount--;
                SetAnim("GroundAttack");
            }
        }
        _contactTime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : 땅에 tag 붙이기
        // if(collision.collider.tag == "Ground")
        //{
        
        if(collision.collider.TryGetComponent(out BoxCollider2D boxColllider)) // 정빈님 바닥의 태그를 잡던지 해서
        {
            IsAir = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Note note) && !IsDamaged && !IsDied)
        {
            SetAnim("OnDamage");
            IsDamaged = true;

            float _dmg = note.GetDamage();
            // TODO : 추후 수정예정
            CurHP -= 1;
            StartCoroutine(Invincibility());
            StartCoroutine(Clicker());
        }
    }
    // 체공 시간 조절 코루틴
    public IEnumerator InAirTime()
    {
        _rigid.isKinematic = true;
        yield return new WaitForSeconds(_inAirTime);
        _rigid.isKinematic = false;
        yield break;
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

    IEnumerator Die()
    {
        // 이벤트 (캐릭터 사망) 등록
        // EventManager.Instance.PlayEvent(E_Event.PlayerDie);
        IsDied = true;
        
        _rigid.position = _startPos;
        SetAnim("Die");
        yield return new WaitForSeconds(0.02f);
        Destroy(_rigid);

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
            IsAir = true;
        if (!state)
            IsAir = false;
    }
}
