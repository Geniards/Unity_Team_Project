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

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 GroundPos { get; private set; }    // 땅의 위치값
    public Vector2 JumPos { get; private set; }       // 점프 위치값
    Vector2 _rigidYPos;                                // 캐릭터 Y 높이값
    [SerializeField] WGH_JudgeCircle _judgeCircle;

    [Header("기타")]
    private bool _isAir;                               // 체공 여부
    Coroutine _IsAirRountine;                          // 체공 코루틴
    
    private void Awake()
    {
        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // 하강하는 느낌이 들게 살짝 위에서 떨어지도록 값 설정 => TODO : 값 인스펙터에서 조절할 수 있도록 변경
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
        if(Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.J))
        {
            _judgeCircle.SetTopCircleOff();
            _judgeCircle.SetMiddleCircleOn();
            _judgeCircle.SetBottomCircleOff();
            SetAnim("GroundAttack");
        }
        else
        {
            _judgeCircle.SetMiddleCircleOff();
            // 점프 키를 눌렀을 경우
            if (!_isAir && Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                _judgeCircle.SetTopCircleOn();      // Top 콜라이더 활성화
                _judgeCircle.SetBottomCircleOff();  // Bottom 콜라이더 비활성화

                _isAir = true;                      // 체공 상태
                SetAnim("Jump");
                _rigid.position = JumPos;           // 캐릭터가 지정한 위치로 순간이동
                // 체공 코루틴
                _IsAirRountine = StartCoroutine(InAirTime());

            }

            // 공격 키를 눌렀을 경우 && 땅에 있을 경우
            if (_isAir == false && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
            {
                // 만약 judgecircle의 노트가 있고

                // 만약 judgecircle의 isperfect가 true라면 OnHit에 perfect를 전달
                // 그게 아니라면(else) great를 전달
                if(_judgeCircle._isGreatCircleIn && _judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                }
                else if(_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Great);
                    Debug.Log("그레잇");
                }
                // 하단 공격
                SetAnim("GroundAttack");
            }

            // 공격 키를 눌렀을 경우 && 공중에 있을 경우
            else if (_isAir && Input.GetKeyDown(KeyCode.J))
            {
                _judgeCircle.SetTopCircleOff();
                _judgeCircle.SetBottomCircleOn();
                if (_IsAirRountine != null)
                {
                    StopCoroutine(_IsAirRountine);
                    _rigid.isKinematic = false;
                }
                // 하강 공격
                SetAnim("FallAttack");
                _rigid.position = GroundPos;
            }
        }
        
    }

    public void Attack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.)
        // if(collision.collider.tag == "Ground")
        //{
        _judgeCircle.SetTopCircleOff();
        _judgeCircle.SetBottomCircleOn();
        _isAir = false;
        //}
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
    /// <summary>
    /// 애니메이션 시작 메서드
    /// </summary>
    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
