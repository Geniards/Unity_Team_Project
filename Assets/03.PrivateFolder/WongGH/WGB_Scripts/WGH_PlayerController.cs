using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("수치조절")]
    [SerializeField] float _inAirTime;                  // 체공시간
    [SerializeField] float _jumpHeight;                 // 점프 시 플레이어의 높이 위치

    [Header("참조")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 _groundPos { get; private set; }    // 땅의 위치값
    public Vector2 _jumPos { get; private set; }       // 점프 위치값
    Vector2 _rigidYPos;                                // 캐릭터 Y 높이값
    [SerializeField] GameObject _judgeCircle;

    [Header("기타")]
    private bool _isAir;                               // 체공 여부
    Coroutine _IsAirRountine;                          // 체공 코루틴
    
    private void Awake()
    {
        _inAirTime = 0.3f;
        _jumpHeight = 5f; 

        // 참조
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _groundPos = new Vector2(transform.position.x, transform.position.y + 0.6f);    // 하강하는 느낌이 들게 살짝 위에서 떨어지도록 값 설정
        _jumPos = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _judgeCircle = FindAnyObjectByType<WGH_JudgeCircle>().gameObject;
    }

    private void Update()
    {
        _rigidYPos = Camera.main.WorldToScreenPoint(_rigid.position);
        if(_rigidYPos.y > Screen.height * 0.5f)
        {
            _judgeCircle.GetComponent<WGH_JudgeCircle>().enabled = false;
        }
        else if(_rigidYPos.y < Screen.height * 0.5f) 
        {
            _judgeCircle.GetComponent<WGH_JudgeCircle>().enabled = true;
        }

        // 점프 키를 눌렀을 경우
        if(!_isAir && Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isAir = true;
            SetAnim("Jump");
            _rigid.position = _jumPos;
            // 체공 코루틴
            _IsAirRountine = StartCoroutine(InAirTime());
            
        }

        // 공격 키를 눌렀을 경우 && 땅에 있을 경우
        if(_isAir == false && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // 하단 공격
            SetAnim("GroundAttack");
        }
        // 공격 키를 눌렀을 경우 && 공중에 있을 경우
        else if (_isAir && Input.GetKeyDown(KeyCode.J))
        {
            if (_IsAirRountine != null)
            {
                StopCoroutine(_IsAirRountine);
                _rigid.isKinematic = false;
            }

            // 하강 공격
            SetAnim("FallAttack");
            _rigid.position = _groundPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.collider.tag == "Ground")
        //{
        _isAir = false;
        //}
        // if(collision.collider.tag == "Monster")
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

    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
