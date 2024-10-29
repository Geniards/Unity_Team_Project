using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("��ġ����")]
    [SerializeField] float _inAirTime;                  // ü���ð�                         / ���� �� : 0.3f
    [SerializeField] float _jumpHeight;                 // ���� �� �÷��̾��� ���� ��ġ      / ���� �� : 5f
    [SerializeField] int _maxHp;
    [SerializeField] int _curHp;
    [SerializeField] float _clikerTime;                 // ������ �ӵ�
    [SerializeField] float _invincivilityTime;          // �����ð�

    [Header("����")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;

    Vector3 _startPos;
    public Vector2 GroundPos { get; private set; }    // ���� ��ġ��
    public Vector2 JumPos { get; private set; }       // ���� ��ġ��

    bool _isFPress;
    bool _isJPress;
    float _fPressTime;
    float _jPressTime;


    private bool _isAir;                               // ü�� ����
    bool _isCanJump = true;
    Coroutine _IsAirRountine;                          // ü�� �ڷ�ƾ
    bool _isDamaged;                                   // �ǰ� ����
    private void Awake()
    {
        _curHp = _maxHp;
        // ����
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // �ϰ��ϴ� ������ ��� ��¦ ������ ���������� �� ����
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
        //    // ���� Ŭ���� �ƴ� ���
        //    else
        //    {
        //        // �ߴ� ���ݶ� �״� kinematic �ٽ� ����
        //        _rigid.isKinematic = false;
        //
        //        // ���� Ű�� ������ ���
        //        if (Input.GetKey(KeyCode.F))
        //        {
        //            _fPressTime += Time.deltaTime;
        //
        //            if (_fPressTime >= 0.05f)
        //            {
        //                _isAir = true;                          // ü�� ����   
        //                if (_isCanJump)
        //                {
        //                    _rigid.position = JumPos;           // ĳ���Ͱ� ������ ��ġ�� �����̵�
        //                    _rigid.isKinematic = true;
        //                    _rigid.isKinematic = false;
        //                    _isCanJump = false;
        //                }
        //                _IsAirRountine = StartCoroutine(InAirTime()); // ü�� �ڷ�ƾ
        //            }
        //        }
        //        else if (Input.GetKeyUp(KeyCode.F))
        //        {
        //            _fPressTime = 0;
        //        }
        //    
        //        // ���� Ű�� ������ ��� && ���� ���� ���
        //        if (!_isAir  && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
        //        {
        //            // �ϴ� ����
        //            SetAnim("GroundAttack");
        //        }
        //        // ���� Ű�� ������ ��� && ���߿� ���� ���
        //        if (_isAir && Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.F))
        //        {
        //            _rigid.position = GroundPos;
        //            
        //            // �ϰ� ����
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
        //    // TODO : �÷��̾� �ǰ� �ִϸ��̼�
        //    SetAnim("Run");
        //}
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : ���� tag ���̱�
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
        // TODO : �ǰ�����
        //}
    }

    // ü�� �ð� ���� �ڷ�ƾ
    IEnumerator InAirTime()
    {
        _rigid.isKinematic = true;
        yield return new WaitForSeconds(_inAirTime);
        _rigid.isKinematic = false;
        yield break;
    }

    // ĳ���� �ǰ� �����Ÿ�
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
    // ����
    IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(_invincivilityTime);
        _isDamaged = false;
        _rigid.isKinematic = false;
        yield break;
    }
    /// <summary>
    /// �ִϸ��̼� ���� �޼���
    /// </summary>
    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
