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

    Vector3 _startPos;
    public Vector2 GroundPos { get; private set; }    // 땅의 위치값
    public Vector2 JumPos { get; private set; }       // 점프 위치값

    bool _isFPress;
    bool _isJPress;
    float _fPressTime;
    float _jPressTime;


    private bool _isAir;                               // 체공 여부
    bool _isCanJump = true;
    Coroutine _IsAirRountine;                          // 체공 코루틴
    bool _isDamaged;                                   // 피격 여부
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
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            _jPressTime = Time.time;
            _isJPress = true;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            _fPressTime = Time.time;
            _isFPress = true;
        }
        if(Mathf.Abs(_jPressTime - _fPressTime) <= 0.2f && _isJPress && _isFPress)
        {
            SetAnim("JumpAttack2");
            _isJPress = false;
            _isFPress = false;
        }
        else
        {
            if(_isJPress && !_isFPress && Input.GetKeyUp(KeyCode.J))
            {
                SetAnim("GroundAttack");
                _isJPress = false;
            }
            if(_isFPress && !_isJPress && Input.GetKeyUp(KeyCode.F))
            {
                _rigid.position = JumPos;
                SetAnim("Jump");
                _isFPress = false;
            }
        }
        //if(!_isAir)
        //{
        //    
        //}
        //if (!_isDamaged)
        //{
        //    if(Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F))
        //    {
        //        _rigid.position = GroundPos;
        //        _rigid.isKinematic = true;
        //        
        //        SetAnim("JumpAttack2");
        //        
        //    }
        //    // 동시 클릭이 아닐 경우
        //    else
        //    {
        //        // 중단 공격때 켰던 kinematic 다시 끄기
        //        _rigid.isKinematic = false;
        //
        //        // 점프 키를 눌렀을 경우
        //        if (Input.GetKey(KeyCode.F))
        //        {
        //            _fPressTime += Time.deltaTime;
        //
        //            if (_fPressTime >= 0.05f)
        //            {
        //                _isAir = true;                          // 체공 상태   
        //                if (_isCanJump)
        //                {
        //                    _rigid.position = JumPos;           // 캐릭터가 지정한 위치로 순간이동
        //                    _rigid.isKinematic = true;
        //                    _rigid.isKinematic = false;
        //                    _isCanJump = false;
        //                }
        //                _IsAirRountine = StartCoroutine(InAirTime()); // 체공 코루틴
        //            }
        //        }
        //        else if (Input.GetKeyUp(KeyCode.F))
        //        {
        //            _fPressTime = 0;
        //        }
        //    
        //        // 공격 키를 눌렀을 경우 && 땅에 있을 경우
        //        if (!_isAir  && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
        //        {
        //            // 하단 공격
        //            SetAnim("GroundAttack");
        //        }
        //        // 공격 키를 눌렀을 경우 && 공중에 있을 경우
        //        if (_isAir && Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.F))
        //        {
        //            _rigid.position = GroundPos;
        //            
        //            // 하강 공격
        //            SetAnim("FallAttack");
        //            if (_IsAirRountine != null)
        //            {
        //                StopCoroutine(_IsAirRountine);
        //                _rigid.isKinematic = false;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    // TODO : 플레이어 피격 애니메이션
        //    SetAnim("Run");
        //}
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
        if(collision.collider.TryGetComponent(out Note note) && !_isDamaged)
        {
            _isDamaged = true;
            _rigid.position = new Vector2(GroundPos.x, GroundPos.y);
            _rigid.isKinematic = true;
            _curHp -= 1;
            StartCoroutine(Invincibility());
            StartCoroutine(Clicker());
        }
        // if(collision.collider.tag == "Monster" || collision.collider.tag == "Obstacle")
        //{
        // TODO : 피격판정
        //}
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
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(_clikerTime);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(_clikerTime);
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(_clikerTime);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield break;
    }
    // 무적
    IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(_invincivilityTime);
        _isDamaged = false;
        _rigid.isKinematic = false;
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
