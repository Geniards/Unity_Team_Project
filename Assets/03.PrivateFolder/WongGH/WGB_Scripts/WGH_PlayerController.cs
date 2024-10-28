using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField] float _inAirTime;                  // 체공시간                         / 기준 값 : 0.3f
    [SerializeField] float _fallAttackHeight;           // 하강 공격 시 플레이어의 높이 위치  / 기준 값 : 0.4f
    [SerializeField] float _jumpHeight;                 // 점프 시 플레이어의 높이 위치      / 기준 값 : 5f
    [SerializeField] int _maxHp;
    [SerializeField] int _curHp;
    [SerializeField] float _invincivilityTime;          // 무적시간

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 GroundPos { get; private set; }    // 땅의 위치값
    public Vector2 JumPos { get; private set; }       // 점프 위치값
    Vector2 _rigidYPos;                                // 캐릭터 Y 높이값
    [SerializeField] WGH_JudgeCircle _judgeCircle;

    [Header("기타")]
    private bool _isAir;                               // 체공 여부
    bool _canJump = true;
    Coroutine _IsAirRountine;                          // 체공 코루틴
    bool _isDamaged;                                   // 피격 여부
    float fpresstime = 0;
    private void Awake()
    {
        _maxHp = 3;
        _curHp = _maxHp;
        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // 하강하는 느낌이 들게 살짝 위에서 떨어지도록 값 설정
        GroundPos = new Vector2(transform.position.x, transform.position.y + _fallAttackHeight);    
        
        JumPos = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _judgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }
    private void Start()
    {
        _judgeCircle.SetTopCircleOff();
        _judgeCircle.SetMiddleCircleOff();
        _judgeCircle.SetBottomCircleOn();

    }
    private void Update()
    {
        if(!_isAir)
        {
            _judgeCircle.SetBottomCircleOn();
        }
        if (!_isDamaged)
        {
            if(Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F))
            {
                _rigid.position = GroundPos;
                _rigid.isKinematic = true;
                _judgeCircle.SetTopCircleOff();
                _judgeCircle.SetMiddleCircleOn();
                _judgeCircle.SetBottomCircleOff();
                SetAnim("JumpAttack2");
                if (_judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                }
                else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Great);
                }
            }
            //else
            //{
            //    // 점프키를 눌렀을 경우
            //    if(!_isAir && Input.GetKeyDown(KeyCode.F) && !Input.GetKey(KeyCode.J))
            //    {
            //        _judgeCircle.SetTopCircleOn();
            //        _judgeCircle.SetMiddleCircleOff();
            //        _judgeCircle.SetBottomCircleOff();
            //        if (!_isAir)
            //        {
            //            _rigid.position = JumPos;
            //        }
            //        _isAir = true;
            //
            //
            //        if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
            //        {
            //            SetAnim("JumpAttack1");
            //            _judgeCircle.note.OnHit(E_NoteDecision.Great);
            //        }
            //        if (_judgeCircle._isPerfectCircleIn)
            //        {
            //            SetAnim("JumpAttack1");
            //            _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
            //        }
            //        
            //        else
            //        {
            //            SetAnim("Jump");
            //        }
            //    }
            //}
            // 동시 클릭이 아닐 경우
            else
            {
                // 중단 공격때 켰던 kinematic 다시 끄기
                _rigid.isKinematic = false;

                // 점프 키를 눌렀을 경우
                if (Input.GetKey(KeyCode.F))
                {
                    fpresstime += Time.deltaTime;
                    
                    if (fpresstime >= 0.05f)
                    {
                        _isAir = true;                          // 체공 상태   
                        if(_canJump)
                        {
                            _rigid.position = JumPos;           // 캐릭터가 지정한 위치로 순간이동
                            _rigid.isKinematic = true;
                            _rigid.isKinematic = false;
                            _canJump = false;
                        }
                        
                        _IsAirRountine = StartCoroutine(InAirTime()); // 체공 코루틴

                        _judgeCircle.SetTopCircleOn();      // Top 콜라이더 활성화
                        _judgeCircle.SetMiddleCircleOff();
                        _judgeCircle.SetBottomCircleOff();  // Bottom 콜라이더 비활성화
                        if (!_judgeCircle._isGreatCircleIn)
                        {
                            SetAnim("Jump");
                        }
                        else if (_judgeCircle._isGreatCircleIn)
                        {
                            SetAnim("JumpAttack1");
                            if (_judgeCircle._isPerfectCircleIn)
                            {
                                _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                            }
                            else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                            {
                                _judgeCircle.note.OnHit(E_NoteDecision.Great);
                            }
                        }
                    }
                }
                else if(Input.GetKeyUp(KeyCode.F))
                {
                    fpresstime = 0;
                }
            
                // 공격 키를 눌렀을 경우 && 땅에 있을 경우
                if (!_isAir  && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    _judgeCircle.SetMiddleCircleOff();
                    if (_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                    }
                    else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Great);
                    }
                    // 하단 공격
                    SetAnim("GroundAttack");
            
                }
                // 공격 키를 눌렀을 경우 && 공중에 있을 경우
                if (_isAir && Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.F))
                {
                    _judgeCircle.SetBottomCircleOn();
                    _judgeCircle.SetMiddleCircleOff();
                    _judgeCircle.SetTopCircleOff();
                    if (_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
            
                    }
                    else if (_judgeCircle._isGreatCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Great);
                    }
                    _rigid.position = GroundPos;
                    
                    // 하강 공격
                    SetAnim("FallAttack");
                    if (_IsAirRountine != null)
                    {
                        StopCoroutine(_IsAirRountine);
                        _rigid.isKinematic = false;
                    }
                }
            }
        }
        else
        {
            // TODO : 플레이어 피격 애니메이션
            SetAnim("Run");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : 땅에 tag 붙이기
        // if(collision.collider.tag == "Ground")
        //{
        _judgeCircle.SetTopCircleOff();
        _judgeCircle.SetBottomCircleOn();
        if(collision.collider.TryGetComponent(out BoxCollider2D boxColllider))
        {
            _canJump = true;
            _isAir = false;
        }
        
        //}
        if(collision.collider.TryGetComponent(out Note note) && !_isDamaged)
        {
            _isDamaged = true;
            _rigid.position = new Vector2(GroundPos.x, GroundPos.y - (_fallAttackHeight-0.1f));
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
        yield return new WaitForSeconds(0.25f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.25f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.25f);
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
