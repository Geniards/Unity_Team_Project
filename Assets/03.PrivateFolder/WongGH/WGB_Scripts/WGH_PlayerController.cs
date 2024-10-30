using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField] float _inAirTime;                  // 체공시간                         / 기준 값 : 0.3f
    [SerializeField] int _maxHp;
    [SerializeField] int _curHp;
    [SerializeField] float _clikerTime;                 // 깜빡임 속도
    [SerializeField] float _invincivilityTime;          // 무적시간

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    [SerializeField] WGH_AreaJudge _judge;
     

    public Vector3 PlayerFrontBoss { get; private set; }
    Vector3 _startPos;

    bool _isFPress;                                    // f 입력 여부
    bool _isJPress;                                    // j 입력 여부
    float _fPressTime;                                 // f 입력 시간을 받을 값
    float _jPressTime;                                 // j 입력 시간을 받을 값

    
    public bool IsDied { get; private set; }           // 사망여부
    public bool IsDamaged { get; private set; }        // 피격 여부
    public bool IsAir { get; private set; }            // 체공 여부
    
    private void Awake()
    {
        _curHp = _maxHp;
        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();  
    }
    private void Start()
    {
        PlayerFrontBoss = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.MIDDLE);
        _startPos = transform.position;
        _judge = FindAnyObjectByType<WGH_AreaJudge>();
    }
    
    private void Update()
    {
        if (_curHp <= 0 && !IsDied)
        {
            StartCoroutine(Die());
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : 땅에 tag 붙이기
        // if(collision.collider.tag == "Ground")
        //{
        
        if(collision.collider.TryGetComponent(out BoxCollider2D boxColllider))
        {
            IsAir = false;
        }
        
        //}
        
        // if(collision.collider.tag == "Monster" || collision.collider.tag == "Obstacle")
        //{
        // TODO : 피격판정
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Note note) && !IsDamaged && !IsDied)
        {
            SetAnim("OnDamage");
            IsDamaged = true;

            _curHp -= 1;
            // TODO : 민성님께 받아올 데미지를 입는 부분
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
