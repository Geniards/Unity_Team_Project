using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField] float _inAirTime;                  // 체공시간                         / 기준 값 : 0.3f
    [SerializeField] float _jumpHeight;                 // 점프 시 플레이어의 높이 위치      / 기준 값 : 5f
    [SerializeField] int _maxHp;
    [SerializeField] int _curHp;
    [SerializeField] float _clikerTime;                 // 깜빡임 속도
    [SerializeField] float _invincivilityTime;          // 무적시간

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    [SerializeField] WGH_RayJudge _judge;
    Vector3 _startPos;
    public Vector2 GroundPos { get; private set; }    // 땅의 위치값
    public Vector2 JumPos { get; private set; }       // 점프 위치값

    bool _isFPress;                                    // f 입력 여부
    bool _isJPress;                                    // j 입력 여부
    float _fPressTime;                                 // f 입력 시간을 받을 값
    float _jPressTime;                                 // j 입력 시간을 받을 값

    public bool IsDied { get; private set; }           // 사망여부
    public bool IsDamaged { get; private set; }        // 피격 여부
    private bool _isAir;                               // 체공 여부
    bool _isCanJump = true;
    Coroutine _IsAirRountine;                          // 체공 코루틴
    
    private void Awake()
    {
        _curHp = _maxHp;
        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // 하강하는 느낌이 들게 살짝 위에서 떨어지도록 값 설정
        GroundPos = transform.position + new Vector3(0, 0.2f, 0);    
        JumPos = transform.position + new Vector3(0, _jumpHeight, 0);
        
    }
    private void Start()
    {
        _startPos = transform.position;
        _judge = FindAnyObjectByType<WGH_RayJudge>();
    }
    
    private void Update()
    {
        if(_curHp <= 0 && !IsDied)
        {
            StartCoroutine(Die());
        }
        if (!IsDied)                                // 캐릭터가 사망상태가 아니면 입력가능
        {
            if (!IsDamaged)                         // 캐릭터가 피격상태가 아니면 입력가능
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    _jPressTime = Time.time;
                    _isJPress = true;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _fPressTime = Time.time;
                    _isFPress = true;
                }
                if (Mathf.Abs(_jPressTime - _fPressTime) <= 0.2f && _isJPress && _isFPress)
                {
                    SetAnim("MiddleAttack");
                    _isJPress = false;
                    _isFPress = false;
                }
                else
                {
                    if (_isJPress && !_isFPress && Input.GetKeyUp(KeyCode.J))
                    {
                        if (!_isAir)
                        {
                            SetAnim("GroundAttack");
                        }
                        else if (_isAir)
                        {
                            SetAnim("FallAttack");
                            _rigid.position = _startPos;
                        }
                        _isJPress = false;
                    }
                    if (_isFPress && !_isJPress && Input.GetKeyUp(KeyCode.F))
                    {
                        if (!_isAir)
                        {
                            StartCoroutine(JumpAnimCheck());
                        }
                        _isFPress = false;
                    }
                }
            }
            else
            {
                Debug.Log("사망");
                return;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : 땅에 tag 붙이기
        // if(collision.collider.tag == "Ground")
        //{
        
        if(collision.collider.TryGetComponent(out BoxCollider2D boxColllider))
        {
            _isCanJump = true;
            _isAir = false;
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
            StartCoroutine(Invincibility());
            StartCoroutine(Clicker());
        }
    }
    // 체공 시간 조절 코루틴
    IEnumerator InAirTime()
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
        gameObject.GetComponent<Collider2D>().enabled = false;
        _rigid.position = _startPos + new Vector3(0, -0.3f, 0);
        SetAnim("Die");
        IsDied = true;
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        yield break;
    }

    IEnumerator JumpAnimCheck()
    {
        yield return new WaitForSeconds(0.1f);
        if (_judge.Note != null)
        {
            _isAir = true;
            _rigid.position = JumPos;
            SetAnim("JumpAttack1");
        }
        else if (_judge.Note == null)
        {
            _isAir = true;
            _rigid.position = JumPos;
            SetAnim("Jump1");
        }
        yield break;
    }
    /// <summary>
    /// 애니메이션 시작 메서드
    /// </summary>
    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
